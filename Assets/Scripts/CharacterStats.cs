
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
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

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

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage=fireDamage.GetValue();
        int _iceDamage=iceDamage.GetValue();
        int _lightningDamage=lightningDamage.GetValue();
        Debug.Log(_fireDamage);
        int totalMagicDamage=_fireDamage+_iceDamage+_lightningDamage+intelligence.GetValue();
        totalMagicDamage-=_targetStats.magicResistance.GetValue()+_targetStats.intelligence.GetValue();
        totalMagicDamage=Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        Debug.Log(totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage);

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        if (Mathf.Max(_iceDamage, _fireDamage, _lightningDamage) <= 0)
        {
            return;
        }

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if(Random.value<.3f&&_fireDamage > 0)
            {
                canApplyIgnite=true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyChill);
                Debug.Log("Applied fire");
            }
            if (Random.value < .3f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyChill);
                Debug.Log("Applied ice");
            }
            if (Random.value < .3f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyChill);
                Debug.Log("Applied lighting");
            }
        }
    }
    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
        if (isChilled|| isChilled || isShocked)
        {
            return;
        }
        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
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
        DoMagicDamage(_targetStats);
        Debug.Log(totalDamage);
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

