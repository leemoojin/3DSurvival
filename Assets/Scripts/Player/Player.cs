using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;

    //itemData 에 인터렉션되는 아이템 데이터를 넣어준다
    public ItemData itemData;

    //addItem(델리게이트)에 구독된 함수를 실행
    public Action addItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        //같은 위치에 있다면 GetComponent로 값을 찾을 수 있다
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
