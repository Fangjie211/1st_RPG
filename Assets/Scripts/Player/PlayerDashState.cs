using System.Collections;
using System.Collections.Generic;
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
        SkillManager.instance.clone.CreateClone(player.transform);
        enemy = GameObject.Find("Enemy_skeleton").transform;
        stateTimer = player.dashDuration;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(),enemy.GetComponent<Collider2D>());
    }

    public override void Exit()
    {
        rb.velocity=new Vector2(0,rb.velocity.y);
        base.Exit();
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>(),false);
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
