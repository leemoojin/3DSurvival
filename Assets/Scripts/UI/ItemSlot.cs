using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{   
    // 슬롯창에 들어갈 아이템 정보
    public ItemData item;
    // 인벤토리 정보
    public UIInventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
    private Outline outline;

    // 몇번째 아이템 슬롯인지 정보
    public int index;
    // 장착된건지
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        //icon.gameObject.SetActive(true);
        //icon.sprite = item.icon;
        //quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        //if (outline != null)
        //{
        //    outline.enabled = equipped;
        //}
    }

    // 아이템을 버리거나 소모하면 호출
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}