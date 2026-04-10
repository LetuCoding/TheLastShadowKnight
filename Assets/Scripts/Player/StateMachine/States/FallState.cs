using Player.StateMachine;
using UnityEngine;

namespace Player.StateMachine.States
{
    /// <summary>
    /// Active while the player is airborne and moving downward (or was pushed off a ledge).
    /// Handles transitions to Idle (landing), WallState (wall contact), and DashState.
    /// </summary>
    public class FallState : PlayerState
    {
        public FallState(PlayerStateMachine fsm, Player player, InputSystem_Actions inputActions)
            : base(fsm, player, inputActions) { }

        // ──────────────────────────────────────────────────────────────────────────────

        public override void Enter()
        {
            // Heavier gravity on the way down for a snappier feel.
            Player.MovementComponent.SetGravity(3.5f);
        }

        public override void Exit() { }

        public override void LogicUpdate()
        {
            if (Player.DashPressed && Player.CanDash)
            {
                Fsm.ChangeState(Player.DashState);
                return;
            }

            if (Player.IsOnWall)
            {
                Fsm.ChangeState(Player.WallState);
                return;
            }

            if (Player.IsGrounded)
            {
                Fsm.ChangeState(Player.IdleState);
            }
        }

        public override void PhysicsUpdate() { }
    }
}