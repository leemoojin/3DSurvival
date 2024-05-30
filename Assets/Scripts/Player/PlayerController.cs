using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //���� ���� ������ Ÿ��Ʋ ����
    [Header("Movement")]
    public float moveSpeed;
    //��ǲ�׼�(�̵�)���� �޾ƿ� ��
    private Vector2 curMovementInput;
    public float jumptForce;
    //�÷��̾ ������󿡼� �����ϱ� ����
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    //ȸ�� ����
    public float minXLook;
    public float maxXLook;
    //��ǲ�׼ǿ��� �޾ƿ� ���콺�� ��Ÿ��
    private float camCurXRot;
    //���콺 ȸ���Ҷ� �ΰ���
    public float lookSensitivity;
    //���콺 ��Ÿ��
    private Vector2 mouseDelta;

    [HideInInspector]
    // �κ��� �������� ������ ���� ���콺 Ŀ���� �ʿ�
    // ȭ���� �����̸� �ȵ�, �װ��� ��Ʈ���ϱ� ���� �Ұ�
    public bool canLook = true;

    //������ �κ� ��� �Լ� ������ ���� ��������Ʈ
    public Action inventory;

    private Rigidbody rigidbody;

    private Interaction interaction;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        interaction = GetComponent<Interaction>();
    }

    void Start()
    {   
        //���콺 Ŀ�� �����
        Cursor.lockState = CursorLockMode.Locked;
    }

    //���������� FixedUpdate �� �ϴ°� ����
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {   
        //���콺�� ���� ��ȭ�� ��� �������̱⶧����
        //����� �޸� ���� ������ ���°� �ʿ����
        //���� �о����ȴ�
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        //context.phase �� �Է��� ������¸� �޾ƿ´�
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            //Impulse : �������� ���� �ִ� ���
            rigidbody.AddForce(Vector2.up * jumptForce, ForceMode.Impulse);
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {   
        // ��Ű�� ��������
        if (context.phase == InputActionPhase.Started)
        {
            // UIInventory �� ����Լ� �̵��� ���� ��������Ʈ�� ���
            // inventory �� ��ϵ� �Լ� ȣ��
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    // e Ű�� �Ĺ����� ��
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        //Debug.Log($"PlayerController.cs - OnInteractInput() - e �Է�");

        // Ű�� �� �Է����� ��
        if (context.phase == InputActionPhase.Started)
        {
            interaction.OnInteract();
        }
    }


    private void Move()
    {
        //���Ⱚ�� ����
        //forward : w, s 
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        //������ ���� ���� ���Ʒ��� �̵�
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        //y���� ������ �¿�� �����δ�
        //���콺 ��Ÿ������ �Է¹��� �¿� �̵����� x����
        //ī�޶� y�࿡ �־�� ���ϴ� ȿ���� ��´�
        //x���� y�� �ִ´�

        camCurXRot += mouseDelta.y * lookSensitivity;
        //�ּҰ� �ִ밪�� �Ѿ�� �ʰ�
        //�ּҰ����� �۾����� �ּҰ��� ����
        //�ִ밪���� Ŀ���� �ִ밪 ����
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        //ī�޶�� ���� ��ǥ�� �ƴ϶� ���� ��ǥ�� �����ش�
        //�Է¹��� y���� x�� �ִ´�
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        
        //x���� y�� �ִ´�
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        //�÷��̾ �������� å��ٸ� 4���� ����ٰ� ��������
        Ray[] rays = new Ray[4]
        {   
            //
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleCursor()
    {
        // �⺻ ���´� CursorLockMode.Locked - start()���� ����
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        // CursorLockMode�� ���̸� ���� Ǯ���ش�, Ǯ���ִٸ� ���� �Ǵ�
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        // toggle �� true�϶� CursorLockMode.None ���ǰ� ȭ���� �������� �ʴ´�
        canLook = !toggle;
    }
}