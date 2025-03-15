using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList=new List<ItemData>();


    public virtual void GenerateDrop()
    {
        for(int i=0; i<possibleItemDrop; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i%possibleDrop.Length].dropChance)
            {
                dropList.Add(possibleDrop[i%possibleDrop.Length]);
            }
        }
        for(int i = 0; i < dropList.Count; i++)
        {
            ItemData randomItem=dropList[Random.Range(0, dropList.Count-1)];
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }
    protected void DropItem(ItemData _itemData)
    {

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        GameObject drop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        drop.GetComponent<ItemObject>().SetupItem(_itemData,randomVelocity);
    }   


}
