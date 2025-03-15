using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public ItemEffect[] itemEffects;

    [Header("Major Stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive Stats")]
    public int damage;
    public int critPower;
    public int critChance;

    [Header("Defensive Stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;


    [Header("Craft requirements")]
    public List<InventoryItem> craftMaterials;

    public float itemCooldown;
    public void AddModifiers()
    {
        PlayerStats playerStats=PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);
        playerStats.damage.AddModifier(damage);
        playerStats.critPower.AddModifier(critPower);
        playerStats.critChance.AddModifier(critChance);
        playerStats.maxHp.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }


    public void RemoveModifiers() {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        playerStats.damage.RemoveModifier(damage);
        playerStats.critPower.RemoveModifier(critPower);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.maxHp.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);

    }
    public void ExecuteItemEffect(Transform _enemyPosition)
    {
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }
    public override string GetDescription()
    {
        sb.Length = 0;
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");
        AddItemDescription(damage, "Damage");
        AddItemDescription(critPower, "Crit Power");
        AddItemDescription(critChance, "Crit Chance");
        AddItemDescription(health, "Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic Resistance");
        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightningDamage, "Lightning Damage");

        return sb.ToString();
    }

    private void AddItemDescription(int _value,string _name)
    {
        if(_value!=0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }
            sb.Append("+ "+_value+" "+_name);
        }
    }
}
