using Player.StateMachine;
using UnityEngine;


namespace Player.StateMachine.States
{
    /// <summary>
    /// The default grounded state. The player can transition to Jump, Fall, or Dash
    /// from here.
    /// </summary>
    public class IdleState : PlayerState
    {
        public IdleState(PlayerStateMachine fsm, Player player, InputSystem_Actions inputActions)
            : base(fsm, player, inputActions) { }

        // ──────────────────────────────────────────────────────────────────────────────

        public override void Enter()
        {
            Player.MovementComponent.SetGravity(3.5f);
        }

        public override void Exit() { }

        public override void LogicUpdate()
        {
            // Dash has the highest priority while a charge is available.
            if (Player.DashPressed && Player.CanDash)
            {
                Fsm.ChangeState(Player.DashState);
                return;
            }

            if (Player.AttackPressed)
            {
                Fsm.ChangeState(Player.AttackState);
            }

            if (Player.JumpPressed)
            {
                Fsm.ChangeState(Player.JumpState);
                return;
            }

            // Walked off a ledge.
            if (!Player.IsGrounded)
            {
                Fsm.ChangeState(Player.FallState);
            }
        }

        public override void PhysicsUpdate() { }
    }
}