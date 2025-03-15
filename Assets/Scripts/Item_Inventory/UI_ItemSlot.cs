using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    public InventoryItem item;
    private UI ui;
    private void Start()
    {
        ui=GetComponentInParent<UI>();
    }

    // Start is called before the first frame update

    public void UpdateSlot(InventoryItem _item)
    {
        item = _item;

        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite=null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(item== null)
        {
            return;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;
        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null)
            return;
        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }
}
