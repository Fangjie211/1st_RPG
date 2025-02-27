using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExistTimer;
    private Animator anim=>GetComponent<Animator>();
    private CircleCollider2D cd=>GetComponent<CircleCollider2D>();


    private Player player;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;
    private bool canGrow;
    private float growSpeed;
    private Transform closestEnemy;
    [SerializeField] private LayerMask WhatIsEnemy;
    public void SetupCrystal(float _crystalDuration,bool _canExplode,bool _canMove,float _moveSpeed,float _growSpeed,Transform _closestEnemy,Player _player)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        growSpeed = _growSpeed;
        closestEnemy = _closestEnemy;
        player = _player;
    }
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackHoleRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius,WhatIsEnemy);
        if(colliders.Length>0)
        closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }
        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);
        
            if(Vector2.Distance(transform.position, closestEnemy.position) < .1f)
            {
                FinishCrystal();
                canMove=false;
            }
        }
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }

    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        SelfDestroy();
    }
    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                player.stats.DoMagicDamage(hit.GetComponent<CharacterStats>());
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
