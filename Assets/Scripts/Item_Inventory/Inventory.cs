using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour,ISavemanager
{
    public static Inventory instance;


    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDic;

    public List<InventoryItem> EquipItems;
    public Dictionary<ItemData, InventoryItem> EquipDictionary;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<ItemData> startItems;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform statSlotParent;
    [SerializeField] private Transform equipSlotParent;
    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashSlots;
    private UI_EquipmentSlot[] equipSlots;
    private UI_StatSlot[] statSlots;

    [Header("Data base")]
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquip;

    [Header("Items cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;
    public void UseFlask()
    {
        ItemData_Equipment flask=GetEquipment(EquipmentType.Flask);
        if (flask == null)
        {
            return;
        }
        bool canUseFlask = Time.time > lastTimeUsedFlask + flask.itemCooldown;
        if (canUseFlask)
        {
            flask.ExecuteItemEffect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask is on cooldown");
        }
    }
    public bool CanUseArmor()
    {
        ItemData_Equipment armor = GetEquipment(EquipmentType.Armor);
        if (Time.time > lastTimeUsedArmor+armor.itemCooldown)
        {
            lastTimeUsedArmor = Time.time;
            return true;
        }
        Debug.Log("Armor is on cooldown");
        return false;
    }

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


        equipment = new List<InventoryItem>();
        equipmentDic = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipSlots = equipSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlots=statSlotParent.GetComponentsInChildren<UI_StatSlot>();
        AddStartItems();
    }

    private void AddStartItems()
    {
        foreach(ItemData_Equipment item in loadedEquip)
        {
            EquipItem(item);
        }
        if (loadedItems.Count > 0)
        {
            foreach(InventoryItem item in loadedItems)
            {
                for(int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
        }
        else
        {
            for (int i = 0; i < startItems.Count; i++)
            {
                AddItem(startItems[i]);
            }
        }
        return;

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

        for (int i = 0; i < equipSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDic)
            {
                if (item.Key.equipmentType == equipSlots[i].equipmentType)
                    equipSlots[i].UpdateSlot(item.Value);
            }
        }
        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < stashSlots.Length; i++)
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
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment&&CanAddItem())
        {
            AddToEquipment(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }
        UpdateUISlot();
    }
    public bool CanAddItem()
    {
        if (EquipItems.Count>= inventoryItemSlots.Length)
        {
            Debug.Log("Inventory is full");
            return false;
        }
        return true;
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
        if (EquipDictionary.TryGetValue(_item, out InventoryItem value))
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

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requireMaterials)
    {

        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        for (int i = 0; i < _requireMaterials.Count; i++)
        {

            if (stashDictionary.TryGetValue(_requireMaterials[i].data, out InventoryItem item))
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

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);
        Debug.Log("Here is " + _itemToCraft.name);
        return true;
    }

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDic)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
            }
        }
        return equipedItem;

    }


    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stashItems;

    public void LoadData(GameData _data)
    {

        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {

            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemTOload = new InventoryItem(item);
                    itemTOload.stackSize = pair.Value;
                    loadedItems.Add(itemTOload);
                }
            }
        }
        foreach(string itemID in _data.equip)
        {
            foreach(var item in GetItemDataBase())
            {
                if(item!=null && itemID == item.itemID)
                {
                    loadedEquip.Add(item as ItemData_Equipment);
                }
            }
        }
    }


    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equip.Clear();


        foreach(KeyValuePair<ItemData,InventoryItem> pair in EquipDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }
        foreach(KeyValuePair<ItemData,InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }
        foreach(KeyValuePair<ItemData_Equipment,InventoryItem> pair in equipmentDic)
        {
            _data.equip.Add(pair.Key.itemID);
        }
    }

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase=new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/items" });
        foreach (string assetName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(assetName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }
        return itemDataBase;
    }
}
