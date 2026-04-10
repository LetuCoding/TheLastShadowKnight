using Player.StateMachine;
using UnityEngine;

namespace Player.StateMachine.States
{
    /// <summary>
    /// Active while the player is pressed against a wall and not grounded.
    /// The player slides down slowly. Pressing Jump triggers a wall jump.
    /// Losing wall contact transitions to FallState.
    /// </summary>
    public class WallState : PlayerState
    {
        // ──────────────────────────────────────────────────────────────────────────────
        #region Configuration

        /// <summary>Downward slide speed while hugging the wall (units/second).</summary>
        private const float WallSlideSpeed = 1f;

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────

        public WallState(PlayerStateMachine fsm, Player player, InputSystem_Actions inputActions)
            : base(fsm, player, inputActions) { }


        // ──────────────────────────────────────────────────────────────────────────────
        #region State Lifecycle

        public override void Enter()
        {
            // Zero vertical momentum so the slide speed is applied cleanly.
            Player.MovementComponent.SetVelocityY(0f);

            // Touching a wall restores the dash charge, same as touching the ground.
            Player.CanDash = true;

            // Reduce gravity so the slide is controlled rather than a free-fall.
            Player.MovementComponent.SetGravity(0f);
        }

        public override void Exit()
        {
            Player.MovementComponent.SetGravity(3.5f);
        }

        public override void LogicUpdate()
        {
            if (Player.JumpPressed)
            {
                // Player.WallJump() applies the impulse and starts the lockout coroutine
                // before we transition, so JumpState receives the correct IsWallJumping flag.
                Player.WallJump();
                Fsm.ChangeState(Player.JumpState);
                return;
            }

            // Detached from wall (pushed away or fell off).
            if (!Player.IsOnWall)
            {
                Fsm.ChangeState(Player.FallState);
            }
        }

        public override void PhysicsUpdate()
        {
            // Apply a constant slow downward velocity (slide).
            Player.MovementComponent.SetVelocityY(-WallSlideSpeed);
        }

        #endregion
    }
}