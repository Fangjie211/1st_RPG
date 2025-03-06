using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;


    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDic;

    public List<InventoryItem> EquipItems;
    public Dictionary<ItemData, InventoryItem> EquipDictionary;

    public List <InventoryItem>stashItems;
    public Dictionary <ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;

    [SerializeField] private Transform equipSlotParent;
    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashSlots;
    private UI_EquipmentSlot[] equipSlots;


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


        equipment= new List<InventoryItem>();
        equipmentDic = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlots=inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashSlots=stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipSlots=equipSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }


    public void EquipItem(ItemData item)
    {

        ItemData_Equipment newEquip = item as ItemData_Equipment;

        InventoryItem newItem = new InventoryItem(newEquip);

        ItemData_Equipment ItemTODelete = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> _item in equipmentDic)
        {
            if (_item.Key.equipmentType == newEquip.equipmentType)
            {
                ItemTODelete = _item.Key;
            }
        }
        if (ItemTODelete != null)
        {
            UnEquipItem(ItemTODelete);
            AddItem(ItemTODelete);
        }

        equipment.Add(newItem);
        equipmentDic.Add(newEquip, newItem);
        newEquip.AddModifiers();
        RemoveItem(item);
    }

    public void UnEquipItem(ItemData_Equipment ItemTODelete)
    {
        if (equipmentDic.TryGetValue(ItemTODelete, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDic.Remove(ItemTODelete);
            ItemTODelete.RemoveModifiers();
        }
    }

    private void UpdateUISlot()
    {

        for(int i = 0; i < equipSlots.Length; i++)
        {
            foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDic)
            {
                if (item.Key.equipmentType == equipSlots[i].equipmentType)
                    equipSlots[i].UpdateSlot(item.Value);
            }
        }
        for(int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }
        for(int i=0; i < stashSlots.Length; i++)
        {
            stashSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < EquipItems.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(EquipItems[i]);
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

    public bool CanCraft(ItemData_Equipment _itemToCraft,List<InventoryItem> _requireMaterials)
    {

        List<InventoryItem> materialsToRemove=new List<InventoryItem>();
        for(int i=0;i<_requireMaterials.Count;i++)
        {

            if (stashDictionary.TryGetValue(_requireMaterials[i].data,out InventoryItem item))
            {
                //add

                if (item.stackSize < _requireMaterials[i].stackSize)
                {
                    Debug.Log("not enough");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(item);
                }
            }
            else
            {
                Debug.Log("not enough");
                return false;
            }
        }

        for(int i=0;i<materialsToRemove.Count;i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);
        Debug.Log("Here is " + _itemToCraft.name);
        return true;
    }

}
