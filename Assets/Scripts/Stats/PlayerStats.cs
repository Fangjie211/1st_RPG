
using UnityEngine;

public class PlayerStats : CharacterStats
{

    private Player player;
    protected override void Start()
    {
        base.Start();
        player=GetComponent<Player>();
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        Debug.Log(_damage);
    }
    protected override void Die()
    {
        
        base.Die();
        player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null)
        {
            Debug.Log("Armor effect");
            currentArmor.ExecuteItemEffect(player.transform);
        }
    }
    public void CloneDoDamage(CharacterStats _stats,float _multiplier)
    {
        if (TargetCanEvade(_stats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();
        if (_multiplier > 0)
        {
            totalDamage=Mathf.RoundToInt(totalDamage*_multiplier);
        }
        if (CanCrit())
        {
            CalculateCritialDamage(totalDamage);
        }
        totalDamage = CheckTargetArmor(_stats, totalDamage);
        _stats.TakeDamage(totalDamage);
        DoMagicDamage(_stats);
    }
}
