using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive crysyal")]
    [SerializeField] private bool canExplode;
    [SerializeField] private float growSpeed;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
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
                SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, growSpeed, FindClosestEnemy(newCrystal.transform));
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
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, growSpeed, FindClosestEnemy(currentCrystal.transform));
    }
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    protected override void Start()
    {
        base.Start();
    }

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
