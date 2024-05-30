using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    // 인터렉션을 만들때 카메라를 기준으로 Ray를 쏜다
    // 얼마나 자주 업데이트를 해서 검출 할지
    // 한번 검출했을 때 없을 수 도 있다

    // 얼마나 자주 검출할지
    public float checkRate = 0.05f;
    // 마지막으로 검출한 시간
    private float lastCheckTime;
    // 얼마나 멀리있는 것을  체크할지
    public float maxCheckDistance;
    // 어떤 레이어가 달려있는 게임 오브젝트를 추출할지
    public LayerMask layerMask;

    // 인터렉션에 성공해서 현재 인터렉션된 오브젝트
    public GameObject curInteractGameObject;
    //만들어둔 인터페이스를 캐싱
    private IInteractable curInteractable;

    // 프롬포트에 띄워줄 ui
    // 직접 오브젝트 할당 없이, ui분리방법 생각해볼것*
    // 드래그앤 드롭이라 아닌 출력방법 고민해볼것*
    public TextMeshProUGUI promptText;
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 실시간으로 상호작용 할 수 있도록 Ray를 출력
        // 정해진 시간 간격에만 체크
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        // 카메라 기준으로 Ray 를 쏜다
        // ScreenPointToRay 카메라 기준 (정중앙으로 설정)
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            //이미 상호작용에 성공해서 데이터를 curInteractGameObject에 담았다면 무시
            if (hit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = hit.collider.gameObject;
                // 상호작용에 성공한 오브젝트의 IInteractable인터페이스 획득 
                //curInteractable = hit.collider.GetComponent<IInteractable>();

                if (curInteractGameObject == null) 
                {
                    Debug.Log("Interaction.cs - PerformRaycast() - curInteractGameObject 없음");

                }

                //컴포넌트를 찾는데 실패하는 현상이 발생->캠프 파이어의 레이어가 인터렉터블로 되어있어서 발생한 문제
                if (hit.collider.TryGetComponent<IInteractable>(out curInteractable))
                {
                    SetPromptText();
                }
                else 
                {
                    Debug.Log(curInteractGameObject.name);

                    Debug.Log("Interaction.cs - PerformRaycast() - curInteractable 없음");
                    curInteractGameObject = null;
                    curInteractable = null;
                    promptText.gameObject.SetActive(false);
                }
                
            }
        }
        else
        {
            // 레이가 오브젝트 탐색에 실패 했을 때 
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }

    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        //이미 캐싱되어있는 IInteractable 를 사용
        promptText.text = curInteractable.GetInteractPrompt();
    }


    //E 를 입력해서 상호작용 했을 때 - 플레이어 컨트롤러로 이동하는 리펙토링 고민*
    public void OnInteractInput(InputAction.CallbackContext context)
    {   
        // 키를 막 입력했을 때 &&  상호작용에 성공했을 때
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}