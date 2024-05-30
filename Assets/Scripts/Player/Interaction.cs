using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    // ���ͷ����� ���鶧 ī�޶� �������� Ray�� ���
    // �󸶳� ���� ������Ʈ�� �ؼ� ���� ����
    // �ѹ� �������� �� ���� �� �� �ִ�

    // �󸶳� ���� ��������
    public float checkRate = 0.05f;
    // ���������� ������ �ð�
    private float lastCheckTime;
    // �󸶳� �ָ��ִ� ����  üũ����
    public float maxCheckDistance;
    // � ���̾ �޷��ִ� ���� ������Ʈ�� ��������
    public LayerMask layerMask;

    // ���ͷ��ǿ� �����ؼ� ���� ���ͷ��ǵ� ������Ʈ
    public GameObject curInteractGameObject;
    //������ �������̽��� ĳ��
    private IInteractable curInteractable;

    // ������Ʈ�� ����� ui
    // ���� ������Ʈ �Ҵ� ����, ui�и���� �����غ���*
    // �巡�׾� ����̶� �ƴ� ��¹�� ����غ���*
    public TextMeshProUGUI promptText;
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // �ǽð����� ��ȣ�ۿ� �� �� �ֵ��� Ray�� ���
        // ������ �ð� ���ݿ��� üũ
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        // ī�޶� �������� Ray �� ���
        // ScreenPointToRay ī�޶� ���� (���߾����� ����)
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            //�̹� ��ȣ�ۿ뿡 �����ؼ� �����͸� curInteractGameObject�� ��Ҵٸ� ����
            if (hit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = hit.collider.gameObject;
                // ��ȣ�ۿ뿡 ������ ������Ʈ�� IInteractable�������̽� ȹ�� 
                //curInteractable = hit.collider.GetComponent<IInteractable>();

                if (curInteractGameObject == null) 
                {
                    Debug.Log("Interaction.cs - PerformRaycast() - curInteractGameObject ����");

                }

                //������Ʈ�� ã�µ� �����ϴ� ������ �߻�->ķ�� ���̾��� ���̾ ���ͷ��ͺ�� �Ǿ��־ �߻��� ����
                if (hit.collider.TryGetComponent<IInteractable>(out curInteractable))
                {
                    SetPromptText();
                }
                else 
                {
                    Debug.Log(curInteractGameObject.name);

                    Debug.Log("Interaction.cs - PerformRaycast() - curInteractable ����");
                    curInteractGameObject = null;
                    curInteractable = null;
                    promptText.gameObject.SetActive(false);
                }
                
            }
        }
        else
        {
            // ���̰� ������Ʈ Ž���� ���� ���� �� 
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }

    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        //�̹� ĳ�̵Ǿ��ִ� IInteractable �� ���
        promptText.text = curInteractable.GetInteractPrompt();
    }


    //E �� �Է��ؼ� ��ȣ�ۿ� ���� �� - �÷��̾� ��Ʈ�ѷ��� �̵��ϴ� �����丵 ���*
    public void OnInteractInput(InputAction.CallbackContext context)
    {   
        // Ű�� �� �Է����� �� &&  ��ȣ�ۿ뿡 �������� ��
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}