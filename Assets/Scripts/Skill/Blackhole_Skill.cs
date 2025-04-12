using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{
    public bool blackholeUnlocked {  get; private set; }
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    private BlackHole_Skill_Controller currentBlackhole;
    [SerializeField] private GameObject blackHolePrefab;

    private void UnlockBlackhole()
    {
        if (blackHoleUnlockButton.unlocked)
        {
            blackholeUnlocked = true;
        }
    }

    
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole=Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);
        currentBlackhole=newBlackHole.GetComponent<BlackHole_Skill_Controller>();
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed,amountOfAttacks, cloneCooldown,blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }
    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
    public bool SkillCompleted()
    {
        if(!currentBlackhole)
            return false;
        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockBlackhole();
       
    }
}
