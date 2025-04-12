using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Crystal simple")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Crystal Mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;
    [SerializeField] private UI_SkillTreeSlot unlockCrystalMirageButton;

    [Header("Explosive crysyal")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;
    [SerializeField] private float growSpeed;

    [Header("Moving crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackingButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField]private List<GameObject> crystalLeft=new List<GameObject>();
    [SerializeField] private float useTimeWindow;

    
    private void ResetAbility()
    {
        if (cooldownTimer > 0)
        {
            return;
        }
        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    private bool canUseMultiCrystal()
    {
        if (canUseMultiStacks) {

            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }
            
               
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn,player.transform.position,Quaternion.identity);
                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<Crystal_Skill_Controller>().
                SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, growSpeed, FindClosestEnemy(newCrystal.transform), player);
                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
                return true;
            }
        }
        return false; 
    }
    public override void UseSkill()
    {
        base.UseSkill();
        if (canUseMultiCrystal()) {
            return;
        }
        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {

            if (canMoveToEnemy)
            {
                return;
            }
            Vector2 playerPos= player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position= playerPos;



            if (cloneInsteadOfCrystal)
            {
                player.skill.clone.CreateClone(currentCrystal.transform,Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, growSpeed, FindClosestEnemy(currentCrystal.transform),player);
    }
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    protected override void Start()
    {
        base.Start();
        unlockMultiStackingButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockCrystalMirageButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);


    }
    #region Unlock skill region

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
        UnlockMovingCrystal();
        UnlockMultiStack();
    }
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }
    private void UnlockCrystalMirage()
    {
        if (unlockCrystalMirageButton.unlocked)
        {
            cloneInsteadOfCrystal = true;
        }
    }
    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
        {
            canExplode = true;
        }
    }
    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
        {
            canMoveToEnemy = true;
        }
    }
    private void UnlockMultiStack()
    {
        if (unlockMultiStackingButton.unlocked)
        {
            canUseMultiStacks= true;
        }
    }
    #endregion
    protected override void Update()
    {
        base.Update();
    }


    private void RefilCrystal()
    {

        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
        
    }
}
