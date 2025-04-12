
using System.Collections;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightningDamage
}
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


    public bool isDead { get; private set; }
    public bool isVulnerable { get; private set; }
    private int shockDamage;
    [SerializeField] private GameObject ShockPrefab;
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
        if (chilledTimer < 0)
        {
            isChilled = false;
        }
        if (shockedTimer < 0)
        {
            isShocked = false;
        }
        if (isIgnited)
        {
            ApplyIgnite();

        }
    }

    private void ApplyIgnite()
    {
        if (igniteDamageTimer < 0)
        {

            DecreaseHealthBy(igniteDamage);
            if (currentHealth < 0&&!isDead)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    #region Magic
    public void SetupIgniteDamage(int damage) => igniteDamage = damage;
    public void SetupShockDamage(int damage) => shockDamage = damage;  
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();


        totalMagicDamage -= _targetStats.magicResistance.GetValue() + _targetStats.intelligence.GetValue();
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        _targetStats.TakeDamage(totalMagicDamage);

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        if (Mathf.Max(_iceDamage, _fireDamage, _lightningDamage) <= 0)
        {
            return;
        }

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage, ref canApplyIgnite, ref canApplyChill, ref canApplyShock);
    }

    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage, ref bool canApplyIgnite, ref bool canApplyChill, ref bool canApplyShock)
    {
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
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
        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamage.GetValue()));
        }
        if (canApplyShock)
        {
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_lightningDamage * .1f));
        }
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {

        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill=!isIgnited&&!isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;
       
        if (_ignite&&canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = 4;
            fx.IgniteFXFor(2);
        }
        if (_chill&&canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = 4;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, 4);
            fx.ChillFXFor(4);
        }
        if (_shock&&canApplyShock )
        {
            if (!isShocked)
            {
                ApplyShock(_shock);

            }
            else
            {

                if (GetComponent<Player>() != null)
                {
                    return;
                }
                HitNearestTargetWithShockStrike();
            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
        {
            return;
        }
        isShocked = _shock;
        shockedTimer = 4;
        fx.ShockFXFor(2);
    }
    #endregion
    private void HitNearestTargetWithShockStrike()
    {
        Transform closestEnemy = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > .1f)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }
        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(ShockPrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ThunderStrikeController>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
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

    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
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

    private IEnumerator VulnerableCoroutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }

    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableCoroutine( _duration));
    }
    public virtual void OnEvasion()
    {

    }
    protected bool TargetCanEvade(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue();
        if (isShocked)
        {
            totalEvasion += 20;
        }
        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }
    protected bool CanCrit()
    {
        int totalCriticalChance =critChance.GetValue();
        if(Random.Range(0, 100) <= totalCriticalChance + agility.GetValue())
        {
            return true;
        }
        return false;
    }
    protected int CalculateCritialDamage(int _damage)
    {
        float totalCritPower=(critPower.GetValue()+strength.GetValue())*.01f;
        float critDamage = _damage + totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        GetComponent<Entity>().DamageEffect();
        if (currentHealth < 0&&!isDead)
        {
            Die();
        }
    }
    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth +=_amount;
        if (currentHealth > GetMaxHealth())
        {
            currentHealth = GetMaxHealth();
        }
        if (OnHealthChanged != null)
        {
            OnHealthChanged();
        }
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
        {
            _damage = Mathf.RoundToInt(_damage*1.5f);
        }
        currentHealth-= _damage;
        if(OnHealthChanged != null)
        {
            OnHealthChanged();
        }
    }
    public virtual void IncreaseStatBy(int _modifier,float _duration,Stat _statToModifier)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModifier));
    }
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModifier)
    {
        _statToModifier.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModifier.RemoveModifier(_modifier);
    }
    public Stat StatOfType(StatType buffType)
    {
        switch (buffType)
        {
            case StatType.strength: return strength;
            case StatType.agility: return agility;
            case StatType.intelligence: return intelligence;
            case StatType.vitality: return vitality;
            case StatType.damage: return damage;
            case StatType.critChance: return critChance;
            case StatType.critPower: return critPower;
            case StatType.health: return    maxHp;
            case StatType.armor: return armor;
            case StatType.evasion: return evasion;
            case StatType.magicRes: return magicResistance;
            case StatType.fireDamage: return fireDamage;
            case StatType.iceDamage: return iceDamage;
            case StatType.lightningDamage: return lightningDamage;
            default: return null;
        }
    }
    protected virtual void Die()
    {
        isDead = true;
    }
    public virtual int GetMaxHealth()
    {
        return maxHp.GetValue()+vitality.GetValue()*5;
    }

}

