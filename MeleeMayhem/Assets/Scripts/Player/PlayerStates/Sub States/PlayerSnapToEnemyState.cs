using UnityEngine;

public class PlayerSnapToEnemyState : PlayerGroundedState
{
    public PlayerSnapToEnemyState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        player.SnapNearEnemy(player.FindClosestEnemy(), playerData.snapDistance);
        StateMachine.ChangeState(player.AttackState);
    }

    public override void Exit()
    {
        base.Exit();

        player.characterController.enabled = true;
    }

}
