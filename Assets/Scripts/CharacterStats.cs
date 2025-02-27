
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    private EntityFX fx;

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
    [SerializeField]private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;

    private int igniteDamage;

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;
    
    public int currentHealth;
    public System.Action OnHealthChanged;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = GetMaxHealth();
        fx= GetComponent<EntityFX>();
    }
    protected virtual void Update()
    {
        igniteDamageTimer -= Time.deltaTime;
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }
        if(chilledTimer < 0)
        {
            isChilled = false;
        }
        if (shockedTimer < 0)
        {
            isShocked = false;
        }
        if (igniteDamageTimer < 0&&isIgnited)
        {

            DecreaseHealthBy(igniteDamage);
            if(currentHealth < 0)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void SetupIgniteDamage(int damage) => igniteDamage = damage;
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage=fireDamage.GetValue();
        int _iceDamage=iceDamage.GetValue();
        int _lightningDamage=lightningDamage.GetValue();
        int totalMagicDamage=_fireDamage+_iceDamage+_lightningDamage+intelligence.GetValue();


        totalMagicDamage-=_targetStats.magicResistance.GetValue()+_targetStats.intelligence.GetValue();
        totalMagicDamage=Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
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
        if (canApplyIgnite) {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamage.GetValue()));
        }
        _targetStats.ApplyAilments(canApplyIgnite,canApplyChill, canApplyShock);
    }
    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
        if (isIgnited|| isChilled || isShocked)
        {
            return;
        }
        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = 4;
            fx.IgniteFXFor(2);
        }
        if (_chill)
        {
            isChilled = _chill;
            chilledTimer = 4;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, 4);
            fx.ChillFXFor(4);
        }
        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = 4; 
            fx.ShockFXFor(2);
        }
    }
    public void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanEvade(_targetStats))
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
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {

        if (_targetStats.isChilled)
        {
            totalDamage-=Mathf.RoundToInt(_targetStats.armor.GetValue()*.8f);
        }
        else
        {
            totalDamage-= _targetStats.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanEvade(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue();
        if (isShocked)
        {
            totalEvasion += 20;
        }
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
        DecreaseHealthBy(_damage);
        if (currentHealth < 0)
        {
            Die();
        }
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth-= _damage;
        if(OnHealthChanged != null)
        {
            OnHealthChanged();
        }
    }

    protected virtual void Die()
    {

    }
    public virtual int GetMaxHealth()
    {
        return maxHp.GetValue()+vitality.GetValue()*5;
    }

}

