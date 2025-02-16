using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;


    [Header("Explosive crysyal")]
    [SerializeField] private bool canExplode;
    [SerializeField] private float growSpeed;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;


    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (currentCrystal == null)
        {
            currentCrystal=Instantiate(crystalPrefab,player.transform.position,Quaternion.identity);
            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            currentCrystalScript.SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,moveSpeed,growSpeed,FindClosestEnemy(currentCrystal.transform));
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

            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
