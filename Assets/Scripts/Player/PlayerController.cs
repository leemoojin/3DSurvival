using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //변수 위에 구분점 타이틀 생성
    [Header("Movement")]
    public float moveSpeed;
    //인풋액션(이동)에서 받아온 값
    private Vector2 curMovementInput;
    public float jumptForce;
    //플레이어를 감지대상에서 제외하기 위해
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    //회전 범위
    public float minXLook;
    public float maxXLook;
    //인풋액션에서 받아올 마우스의 델타값
    private float camCurXRot;
    //마우스 회전할때 민감도
    public float lookSensitivity;
    //마우스 델타값
    private Vector2 mouseDelta;

    [HideInInspector]
    // 인벤이 켜졌을때 선택을 위한 마우스 커서가 필요
    // 화면이 움직이면 안됨, 그것을 컨트롤하기 위한 불값
    public bool canLook = true;

    //유아이 인벤 토글 함수 구독을 위한 델리게이트
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
        //마우스 커서 숨기기
        Cursor.lockState = CursorLockMode.Locked;
    }

    //물리연산은 FixedUpdate 로 하는게 좋다
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
        //마우스는 값의 변화가 계속 진행중이기때문에
        //무브와 달리 시작 진행의 상태가 필요없다
        //값만 읽어오면된다
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        //context.phase 는 입력의 현재상태를 받아온다
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
            //Impulse : 순간적인 힘을 주는 모드
            rigidbody.AddForce(Vector2.up * jumptForce, ForceMode.Impulse);
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {   
        // 탭키를 눌렀을때
        if (context.phase == InputActionPhase.Started)
        {
            // UIInventory 의 토글함수 이동을 위해 델리게이트를 사용
            // inventory 에 등록된 함수 호출
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    // e 키로 파밍했을 때
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        //Debug.Log($"PlayerController.cs - OnInteractInput() - e 입력");

        // 키를 막 입력했을 때
        if (context.phase == InputActionPhase.Started)
        {
            interaction.OnInteract();
        }
    }


    private void Move()
    {
        //방향값을 설정
        //forward : w, s 
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        //점프를 했을 때만 위아래로 이동
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        //y축을 돌려야 좌우로 움직인다
        //마우스 델타값에서 입력받은 좌우 이동값인 x값을
        //카메라 y축에 넣어야 원하는 효과를 얻는다
        //x값은 y에 넣는다

        camCurXRot += mouseDelta.y * lookSensitivity;
        //최소값 최대값이 넘어가지 않게
        //최소값보다 작아지면 최소값을 리턴
        //최대값보다 커지면 최대값 리턴
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        //카메라는 월드 좌표가 아니라 로컬 좌표를 돌려준다
        //입력받은 y값을 x에 넣는다
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        
        //x값은 y에 넣는다
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        //플레이어를 기준으로 책상다리 4개를 만든다고 생각하자
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
        // 기본 상태는 CursorLockMode.Locked - start()에서 설정
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        // CursorLockMode가 락이면 락을 풀어준다, 풀려있다면 락을 건다
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        // toggle 이 true일때 CursorLockMode.None 가되고 화면은 움직이지 않는다
        canLook = !toggle;
    }
}