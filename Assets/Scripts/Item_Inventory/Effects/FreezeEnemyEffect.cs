using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Freeze Enemy Effect", menuName = "Data/Item Effect/Freeze Enemy Effect")]
public class FreezeEnemyEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxHealth() * .1f)
        {
            return;
        }
        if (!Inventory.instance.CanUseArmor()) return;
        Collider2D[] colliders=Physics2D.OverlapCircleAll(_enemyPosition.position, 5);
        foreach (var collider in colliders)
        {
            Debug.Log(collider.name);
            collider.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
