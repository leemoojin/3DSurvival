using UnityEngine;

// ��ȣ�ۿ뿡 �ʿ��� �������̽��� �߰�����
// �������� �þ������ ������ �߰� ���� �ʿ䰡 ��������
public interface IInteractable
{
    public string GetInteractPrompt();

    // ���ͷ�Ʈ �Ǿ����� �߻��ϴ� ȿ��
    // ������ ������Ʈ�� ����Ҷ��� �������� ��ȣ�ۿ��� �����ϰ�
    // ���� ������Ʈ�� ����Ҷ��� �� �ٸ��� ������ �� �ִ�
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{   
    //�̸� ������ ��ũ���ͺ������Ʈ�� �־��� ��
    public ItemData data;

    //ȭ�鿡 ����� ������Ʈ
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    //�̰� ������ ��ȣ�ۿ� �Լ��� ȣ�����ְڴ�
    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;

        //addItem �� ������ ����� �ִٸ� ����
        CharacterManager.Instance.Player.addItem?.Invoke();

        //OnInteract �� ����Ǹ� �������� �κ��丮�� �̵�
        //ȭ�鿡���� ��������
        Destroy(gameObject);
    }
}