using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HealEffect", menuName = "Data/Item Effect/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField]private float healPercent;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
       PlayerStats playerStats=PlayerManager.instance.player.GetComponent<PlayerStats>();
        int healAmount=Mathf.RoundToInt(playerStats.GetMaxHealth() * this.healPercent);
        playerStats.IncreaseHealthBy(healAmount);
    }
}
