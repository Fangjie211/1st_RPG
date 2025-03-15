using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Thunder Strike effect", menuName = "Data/Item Effect/Thunder strike")]
public class ThunderStrikeEffects : ItemEffect
{

    [SerializeField]private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject thunderStrike = Instantiate(thunderStrikePrefab,_enemyPosition.position,Quaternion.identity);
        Destroy(thunderStrike, 1f);
    }
}
