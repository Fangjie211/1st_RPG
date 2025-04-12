using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    
    public bool dashUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot dashUnlockedButton;
    public bool cloneOnDashUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockedButton;
    public bool cloneOnArrivalUnlocked {  get; private set; }
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockedButton;

    public override void UseSkill()
    {
        base.UseSkill();
    }
    
    protected override void Start()
    {
        base.Start();
        dashUnlockedButton.GetComponent<Button>().onClick.AddListener(() => UnlockDash());
        cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(() => UnlockCloneOnDash());
        cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(() => UnlockCloneOnArrival());
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockCloneOnArrival();
        UnlockCloneOnDash();
        UnlockDash();
    }
    private void UnlockDash()
    {
        if(dashUnlockedButton.unlocked)
            dashUnlocked = true;
    }
    private void UnlockCloneOnDash()
    {
        if(cloneOnDashUnlockedButton.unlocked)
        cloneOnDashUnlocked = true;
    }
    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockedButton.unlocked)
        cloneOnArrivalUnlocked = true;
    }
    public void CreateCloneOnDashStart()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
    public void CreateCloneOnDashOver()
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}
