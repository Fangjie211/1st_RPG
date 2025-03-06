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

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name="itemObject"+itemData.itemName;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            rb.velocity= velocity; 
            //
        }
    }


    public void PickUp()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
