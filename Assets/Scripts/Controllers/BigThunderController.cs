using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigThunderController : MonoBehaviour
{
    
   protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {

            PlayerStats playerStats=PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();

            playerStats.DoMagicDamage(enemyStats);
        }
    }
}
