using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInventory : MonoBehaviour
{   
    //아이템 슬롯들의 정보
    public ItemSlot[] slots;

    //인벤토리창
    public GameObject inventoryWindow;
    // 아이템 슬롯 오브젝트들의 부모
    public Transform slotPanel;
    // 버릴 아이템 위치
    public Transform dropPosition;

    //아이템 정보 ui 세팅
    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex;

    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        // 버릴 아이템 위치 초기화
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // 토글을 등록
        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        //처음에는 인벤토리 윈도우를 꺼준다
        //탭키를 눌렀을때만 활성화
        inventoryWindow.SetActive(false);
        //인벤토리 크기를 미리 설정해서 고정
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            //slots[i].Clear();
        }

        //아이템을 선택 안했을 때 설정
        ClearSelectedItemWindow();
    }

    //플레이어 컨트롤러에서 탭키를 눌렀을때 인벤토리창 출력
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        //inventoryWindow 오브젝트가 켜져있는지 여부 출력
        return inventoryWindow.activeInHierarchy;
    }

    // 아이템 파밍, 플레이어가 획득한 아이템이 인벤토리에 추가된다
    public void AddItem()
    {
        // 아이템 데이터를 받아와야한다(파밍한 아이템)
        ItemData data = CharacterManager.Instance.Player.itemData;

        // 파밍한 아이템이 중복가능한지 체크
        if (data.canStack)
        {
            // 중복가능 : 이미 있는 슬롯에 숫자를 더한다
            // 인벤토리에 같은 아이템이 있는지 탐색
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                // 수량 증가
                slot.quantity++;
                UpdateUI();

                // 인벤토리로 데이터를 이동한 후 null 로 비워둔다
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        // 파밍한 아이템이 중복이 아니거나, 새로운 슬롯이 필요할때
        // 빈 슬롯을 가져온다
        ItemSlot emptySlot = GetEmptySlot();
        // 빈 슬롯이 있다면
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();

            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        //빈슬롯이 없다면 아이템을 버린다
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    // 아이템을 버린다
    public void ThrowItem(ItemData data)
    {   
        // 버릴때 다시 프리펩을 생성해준다 , 각도는 360도 중 랜덤
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {   
                // 데이터가 있다면 셋팅해라
                //slots[i].Set();
            }
            else
            {
                // 비어있는 슬롯을 나나태는 ui 로직 호출
                //slots[i].Clear();
            }
        }
    }

    //인벤토리에 같은 아이템이 있는지 탐색
    ItemSlot GetItemStack(ItemData data)
    {   
        for (int i = 0; i < slots.Length; i++)
        {
            // 중복 데이터가 있으며 수량이 최대치 보다 적을때
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    // 아이템이 중복 불가능하면 빈 슬롯을 가져온다
    // 아이템 창이 가득 채워졌다면 null 리턴
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }


    //아이템을 선택 안했을 때 설정
    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    //public void OnUseButton()
    //{
    //    if (selectedItem.item.type == ItemType.Consumable)
    //    {
    //        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
    //        {
    //            switch (selectedItem.item.consumables[i].type)
    //            {
    //                case ConsumableType.Health:
    //                    condition.Heal(selectedItem.item.consumables[i].value); break;
    //                case ConsumableType.Hunger:
    //                    condition.Eat(selectedItem.item.consumables[i].value); break;
    //            }
    //        }
    //        RemoveSelctedItem();
    //    }
    //}

    //public void OnDropButton()
    //{
    //    ThrowItem(selectedItem.item);
    //    RemoveSelctedItem();
    //}

    //void RemoveSelctedItem()
    //{
    //    selectedItem.quantity--;

    //    if (selectedItem.quantity <= 0)
    //    {
    //        if (slots[selectedItemIndex].equipped)
    //        {
    //            UnEquip(selectedItemIndex);
    //        }

    //        selectedItem.item = null;
    //        ClearSelectedItemWindow();
    //    }

    //    UpdateUI();
    //}

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}