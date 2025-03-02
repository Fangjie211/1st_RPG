using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;


    public List<InventoryItem> EquipItems;
    public Dictionary<ItemData, InventoryItem> EquipDictionary;

    public List <InventoryItem>stashItems;
    public Dictionary <ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform equipSlotParent;
    [SerializeField] private Transform stashSlotParent;
    private UI_ItemSlot[] equipSlots;
    private UI_ItemSlot[] stashSlots;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        EquipItems = new List<InventoryItem>();
        EquipDictionary = new Dictionary<ItemData, InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        stashItems = new List<InventoryItem>();

        equipSlots=equipSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashSlots=stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }
    private void UpdateUISlot()
    {
        for (int i = 0; i < EquipItems.Count; i++)
        {
            equipSlots[i].UpdateSlot(EquipItems[i]);
        }
        for (int i = 0; i < stashItems.Count; i++)
        {
            stashSlots[i].UpdateSlot(stashItems[i]);
        }
    }
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            AddToEquipment(_item);
        }
        else if(_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }
        UpdateUISlot();
    }

    private void AddToEquipment(ItemData _item)
    {
        if (EquipDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem item = new InventoryItem(_item);
            EquipItems.Add(item);
            EquipDictionary.Add(_item, item);
        }
        
    }
    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem item = new InventoryItem(_item);
            stashItems.Add(item);
            stashDictionary.Add(_item, item);
        }

    }

    public void RemoveItem(ItemData _item)
    {
         if(EquipDictionary.TryGetValue(_item,out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                EquipItems.Remove(value);
                EquipDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stashItems.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }
        UpdateUISlot();
    }

}
