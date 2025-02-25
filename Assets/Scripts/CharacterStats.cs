
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    [Header("Major Stats")]
    public Stat strength;
    public Stat agility;
    public Stat intelligence;
    public Stat vitality;


    [Header("Defensive Stats")]
    public Stat maxHp;
    public Stat armor;
    public Stat evasion;

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;
    
    [SerializeField] private int currentHealth;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHp.GetValue();
    }


    public void DoDamage(CharacterStats _targetStats)
    {
        if (CanEvade(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
            CalculateCritialDamage(totalDamage);
        }
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool CanEvade(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }
    private bool CanCrit()
    {
        int totalCriticalChance =critChance.GetValue();
        if(Random.Range(0, 100) <= totalCriticalChance + agility.GetValue())
        {
            return true;
        }
        return false;
    }
    private int CalculateCritialDamage(int _damage)
    {
        float totalCritPower=(critPower.GetValue()+strength.GetValue())*.01f;
        float critDamage = _damage + totalCritPower;
        return Mathf.RoundToInt(critDamage);
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

    }
}

