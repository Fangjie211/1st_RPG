using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    public bool restoreUnlocked { get; private set; }
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthAmount;

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked {  get; private set; }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockParry();
        UnlockParryWithMirage();
        UnlockRestoreParry();
    }
    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            player.stats.IncreaseHealthBy(Mathf.RoundToInt(player.stats.GetMaxHealth()*restoreHealthAmount));
        }
    }
    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(() => UnlockParry());
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(() => UnlockParryWithMirage());
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(() => UnlockRestoreParry());
    }
    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
        {
            parryUnlocked = true;
        }
    }
    private void UnlockRestoreParry()
    {
        if (restoreUnlockButton.unlocked)
        {
            restoreUnlocked = true;
        }
    }
    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
        {
            parryWithMirageUnlocked = true;
        }
    }
    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
        {
            SkillManager.instance.clone.CreateCloneOnCounterAttack(_respawnTransform);
        }
    }
}
