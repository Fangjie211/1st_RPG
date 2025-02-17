using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState :PlayerState
{
    private Transform enemy;
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //SkillManager.instance.clone.CreateClone(player.transform,Vector3.zero);
        player.skill.clone.CreateCloneOnDashStart();
        stateTimer = player.dashDuration;
        IgnoreEnemyCollision(true);
        
    }

    public override void Exit()
    {
        player.skill.clone.CreateCloneOnDashOver();
        rb.velocity=new Vector2(0,rb.velocity.y);
        base.Exit();
        IgnoreEnemyCollision(false);
    }
    private void IgnoreEnemyCollision(bool ignore)
    {
        Collider2D playerCollider = rb.GetComponent<Collider2D>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // 先确保敌人有 "Enemy" 标签
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, ignore);
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlide);
        }
        player.SetVelocity(player.dashSpeed*player.dashDir, 0);
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
