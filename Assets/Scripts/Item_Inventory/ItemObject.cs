using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    private SpriteRenderer sr;
    [SerializeField]private Rigidbody2D rb;
    [SerializeField] private Vector2 velocity;

   
    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "itemObject" + itemData.itemName;
    }

    

    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;
        SetupVisuals();
    }
    public void PickUp()
    {
        if(!Inventory.instance.CanAddItem()&&itemData.itemType != ItemType.Equipment)
        {
            rb.velocity=new Vector2(4,4);
            return;
        }
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
