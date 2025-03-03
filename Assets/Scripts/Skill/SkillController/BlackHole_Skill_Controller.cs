using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{


    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> KeyCodeList;


    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;

    private bool canGrow=true;
    private bool canShrink;

    private bool cloneAttackReleased;
    private bool canCreateHotKeys = true;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<GameObject> createdHotKey=new List<GameObject>();
    public List<Transform> targets=new List<Transform>();

    public bool playerCanExitState {  get; private set; }
    private bool playerCanDisappear = true;
    public void SetupBlackhole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _amountOfAttacks,float _cloneAttackCooldown,float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks= _amountOfAttacks;
        cloneAttackCooldown= _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;
        if (SkillManager.instance.clone.crystalInsteadOfClone)
        {
            playerCanDisappear = false;
        }
    }
    
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer=Mathf.Infinity;
            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
                FinishBlackHoleAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }
        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector3(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector3(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);

            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if(targets.Count <=0)
        {
            return;
        }
        cloneAttackReleased = true;
        DestroyHotKey();
        canCreateHotKeys = false;
        if (playerCanDisappear) { 
        playerCanDisappear = false;
        PlayerManager.instance.player.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased&&amountOfAttacks>0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);
            Debug.Log(randomIndex + " " + targets.Count);

            float xOffset;
            if (Random.Range(0, 100) > 50)
            {
                xOffset = 1;
            }
            else
            {
                xOffset = -1;

            }
            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            }
            amountOfAttacks--;


            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility", .9f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {

        DestroyHotKey();
        playerCanExitState = true;
        cloneAttackReleased = false;
        canShrink = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
            // targets.Add(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
           collision.GetComponent <Enemy>().FreezeTime(false);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {

        if (!canCreateHotKeys)
        {
            return;
        }
        if (KeyCodeList.Count <= 0)
        {
            return;
        }
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        createdHotKey.Add(newHotKey);


        KeyCode chosenKey = KeyCodeList[Random.Range(0, KeyCodeList.Count)];
        Debug.Log(chosenKey.ToString());
        KeyCodeList.Remove(chosenKey);

        BlackHoleHotKey_Controller newHotKeyScript = newHotKey.GetComponent<BlackHoleHotKey_Controller>();
        newHotKeyScript.SetupHotKey(chosenKey, collision.transform, this);
    }


    private void DestroyHotKey()
    {
        if (createdHotKey.Count <= 0)
        {
            return;

        }
        for(int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);

}
