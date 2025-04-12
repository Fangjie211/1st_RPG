using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;


    private void OnValidate()
    {
        gameObject.name="EquipmentSlot_"+equipmentType.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if(item == null||item.data==null)
        {
            return;
        }
        //unequip
        Inventory.instance.UnEquipItem(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);
        ui.itemToolTip.HideToolTip();
        CleanUpSlot();
    }
}
