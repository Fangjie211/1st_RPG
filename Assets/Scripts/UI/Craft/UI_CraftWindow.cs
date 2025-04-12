using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField]private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image[] materialImage;

    [SerializeField] private Button craftButton;
    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners();
        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color=Color.clear;

        }

        for(int i = 0;i<_data.craftMaterials.Count;i++)
        {
            if (_data.craftMaterials.Count > materialImage.Length)
            {
                Debug.Log("You have more materials amount than you have");

            }

            materialImage[i].sprite=_data.craftMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();
            materialSlotText.text = _data.craftMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }

        itemDescription.text = _data.GetDescription();
        itemName.text = _data.itemName;
        itemIcon.sprite = _data.icon;

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data,_data.craftMaterials));
    }
}
