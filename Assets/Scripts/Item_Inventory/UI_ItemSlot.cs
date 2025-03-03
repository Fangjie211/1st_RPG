using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    public InventoryItem item;
      
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }
       
    }
}
