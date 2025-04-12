using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    [Range(0,100)]
    public float dropChance;
    public string itemID;
    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID=AssetDatabase.AssetPathToGUID(path);
#endif
    }
    protected StringBuilder sb=new StringBuilder();

    public virtual string GetDescription()
    {
        return "";
    }
}
