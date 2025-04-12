using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    public bool dodgeUnlocked;
    [SerializeField] private int dodgeAmount;
    [Header("Mirage Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool mirageUnlocked;

    protected override void Start()
    {
        base.Start();
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirage);
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockDodge();
        UnlockMirage();
    }
    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked)
        {
            player.stats.evasion.AddModifier(dodgeAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }
    
    private void UnlockMirage()
    {
        if (unlockMirageDodgeButton.unlocked)
        {
            mirageUnlocked = true;
        }
    }

    public void CreateMirageOnDodge()
    {
        if (mirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}
