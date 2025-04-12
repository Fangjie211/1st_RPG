using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private float cloneDuration;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float colorLosingSpeed;


    [Header("Clone Attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;
    //[SerializeField] private bool createCloneOnDashStart;
    //[SerializeField] private bool createCloneOnDashOver;

    [Header("Aggressive Clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    public bool canApplyOnHitEffect { get; private set; }
    [SerializeField] private float aggressiveCloneMultiplier;


    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multipleCloneMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;


    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;
    [SerializeField] private UI_SkillTreeSlot crystalInsteadOfCloneButton;


    #region Unlock region


    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockAggressiveClone();
        UnlockCloneAttack();
        UnlockCrystalInsteadOfClone();
        UnlockMultiClone();
    }
    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multipleCloneMultiplier;
        }
    }

    private void UnlockCrystalInsteadOfClone()
    {
        if (crystalInsteadOfCloneButton.unlocked)
        {
            crystalInsteadOfClone = true;

        }
    }

    #endregion

    protected override void Start()
    {
        base.Start();
        crystalInsteadOfCloneButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
    }
    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {

        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, colorLosingSpeed, canAttack, _offset, FindClosestEnemy(_clonePosition),canDuplicateClone,chanceToDuplicate,player.facingDir,player,attackMultiplier);
    }

    
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
       
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1 * player.facingDir, 0)));
       
    }
    private IEnumerator CreateCloneWithDelay(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
            CreateClone(_transform, _offset);
        
    }
}
