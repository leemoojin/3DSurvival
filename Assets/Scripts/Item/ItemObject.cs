using UnityEngine;

// 상호작용에 필요한 인터페이스를 추가하자
// 아이템이 늘어날때마다 조건을 추가 해줄 필요가 없어진다
public interface IInteractable
{
    public string GetInteractPrompt();

    // 인터렉트 되었을때 발생하는 효과
    // 아이템 오브젝트에 상속할때는 아이템의 상호작용을 정의하고
    // 지형 오브젝트에 상속할때는 또 다르게 정의할 수 있다
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{   
    //미리 만들어둔 스크립터블오브젝트를 넣어줄 것
    public ItemData data;

    //화면에 띄워줄 프롬프트
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    //이걸 누르면 상호작용 함수를 호출해주겠다
    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;

        //addItem 에 구독된 기능이 있다면 실행
        CharacterManager.Instance.Player.addItem?.Invoke();

        //OnInteract 이 실행되면 아이템이 인벤토리로 이동
        //화면에서는 없어진다
        Destroy(gameObject);
    }
}