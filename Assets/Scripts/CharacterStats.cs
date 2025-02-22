using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public Stat damage;
    public Stat strength;
    public Stat maxHp;
    [SerializeField] private int currentHealth;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHp.GetValue();
    }


    public void DoDamage(CharacterStats _targetStats)
    {
        int totalDamage=damage.GetValue()+strength.GetValue();

        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        if (currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        throw new NotImplementedException();
    }
}

