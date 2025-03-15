using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    [SerializeField] private int defaultFontSize = 32;

    public void ShowToolTip(ItemData_Equipment item)
    {


        if(item == null)
        {
            return;
        }   
        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescriptionText.text = item.GetDescription();
        if (itemNameText.text.Length > 12)
        {
            itemNameText.fontSize = 24;
        }
        else
        {
            itemNameText.fontSize = defaultFontSize;
        }
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
