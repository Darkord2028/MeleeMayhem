using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerGroundedState
{
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.characterController.enabled = false;

        if (target == null)
        {
            target = player.FindClosestEnemy();
        }

        if (target != null)
        {
            // 1. Face the enemy instantly
            Vector3 lookAtPos = new Vector3(target.position.x, player.transform.position.y, target.position.z);
            player.transform.LookAt(lookAtPos);

            // 2. Snap-move toward the enemy (optional offset)
            Vector3 targetPosition = new Vector3(target.position.x, player.transform.position.y, target.position.z);

            // Optional: add a small offset so you don't overlap the enemy
            Vector3 direction = (targetPosition - player.transform.position).normalized;
            float snapDistance = 3f; // how far you want to snap
            Vector3 snappedPosition = player.transform.position + direction * snapDistance;

            player.transform.position = snappedPosition - new Vector3(0.5f, 0, 0.5f);
        }

        player.characterController.enabled = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (moveAmount > 0)
        {
            StateMachine.ChangeState(player.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
