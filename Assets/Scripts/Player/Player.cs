using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;

    //itemData �� ���ͷ��ǵǴ� ������ �����͸� �־��ش�
    public ItemData itemData;

    //addItem(��������Ʈ)�� ������ �Լ��� ����
    public Action addItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        //���� ��ġ�� �ִٸ� GetComponent�� ���� ã�� �� �ִ�
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
