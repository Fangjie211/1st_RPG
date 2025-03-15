using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{

    [Header("Player Item Drop")]
    [SerializeField] private float chanceToDrop;
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;
        List<InventoryItem> currentEquipment=inventory.GetEquipmentList();
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();

        List<InventoryItem> currentStash=inventory.GetStashList();
        List<InventoryItem> itemsToDrop = new List<InventoryItem>();

        foreach (InventoryItem item in currentEquipment)
        {
            if (Random.Range(0,100)<=chanceToDrop)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
                //inventory.UnEquipItem(item.data as ItemData_Equipment);
            }
        }
        foreach (InventoryItem item in currentStash)
        {
            DropItem(item.data);
           itemsToDrop.Add(item);
        }
        foreach (InventoryItem item in itemsToDrop)
        {
            inventory.RemoveItem(item.data);
        }
        foreach (InventoryItem item in itemsToUnequip)
        {
            inventory.UnEquipItem(item.data as ItemData_Equipment);
        }
    }
}
