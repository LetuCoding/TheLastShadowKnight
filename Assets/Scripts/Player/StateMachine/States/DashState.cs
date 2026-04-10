using Player.StateMachine;
using UnityEngine;

namespace Player.StateMachine.States
{
    /// <summary>
    /// Short-duration horizontal dash. Gravity is zeroed during the dash so the
    /// trajectory stays flat. Velocity is cleared on exit to avoid carrying momentum.
    /// One dash charge is granted per grounded or wall-contact event.
    /// </summary>
    public class DashState : PlayerState
    {
        // ──────────────────────────────────────────────────────────────────────────────
        #region Configuration

        private const float DashDuration = 0.125f;

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Runtime State

        private float _dashTimer;

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────

        public DashState(PlayerStateMachine fsm, Player player, InputSystem_Actions inputActions)
            : base(fsm, player, inputActions) { }


        // ──────────────────────────────────────────────────────────────────────────────
        #region State Lifecycle

        public override void Enter()
        {
            _dashTimer    = 0f;
            Player.CanDash   = false;
            Player.IsDashing = true;

            Player.MovementComponent.SetGravity(0f);

            float direction = Player.MovementComponent.GetFacingDirection();
            Player.MovementComponent.Dash(direction, Player.dashSpeed);
        }

        public override void Exit()
        {
            Player.IsDashing = false;
            Player.MovementComponent.SetGravity(3.5f);
        }

        public override void LogicUpdate()
        {
            _dashTimer += Time.deltaTime;

            if (_dashTimer >= DashDuration)
            {
                ExitDash();
                return;
            }

            // Allow an early wall-slide transition mid-dash.
            if (Player.IsOnWall)
            {
                Fsm.ChangeState(Player.WallState);
            }
        }

        public override void PhysicsUpdate()
        {
            // Keep vertical velocity at zero for the entire dash duration.
            Player.MovementComponent.SetVelocityY(0f);
        }

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Private Helpers

        /// <summary>
        /// Clears momentum and routes to the appropriate state when the dash ends.
        /// </summary>
        private void ExitDash()
        {
            Player.MovementComponent.SetVelocityY(0f);

            if (Player.IsGrounded)
                Fsm.ChangeState(Player.IdleState);
            else
            {
                Player.MovementComponent.SetVelocityX(0);
                Fsm.ChangeState(Player.FallState);
                
            }
        }

        #endregion
    }
}