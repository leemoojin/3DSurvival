using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;

    //itemData �� ���ͷ��ǵǴ� ������ �����͸� �־��ش�
    public ItemData itemData;

    //�Ĺ� : addItem(��������Ʈ)�� ������ �Լ��� ����
    public Action addItem;

    // ���� ������ ��ġ
    public Transform dropPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        //���� ��ġ�� �ִٸ� GetComponent�� ���� ã�� �� �ִ�
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
