using Player.StateMachine;
using UnityEngine;

namespace Player.StateMachine.States
{
    /// <summary>
    /// Active while the player is moving upward after a jump (normal or wall).
    /// Wall-jump impulse and lockout are handled by Player.WallJump() / WallJumpLockCoroutine,
    /// so this state only needs to manage the upward arc and transition to Fall.
    /// </summary>
    public class JumpState : PlayerState
    {
        // ──────────────────────────────────────────────────────────────────────────────

        public JumpState(PlayerStateMachine fsm, Player player, InputSystem_Actions inputActions)
            : base(fsm, player, inputActions) { }


        // ──────────────────────────────────────────────────────────────────────────────
        #region State Lifecycle

        public override void Enter()
        {
            // Restore gravity in case we came from WallState (which sets it to 0).
            

            // Wall jumps apply their own impulse via Player.WallJump() before
            // transitioning here, so we only call Jump() for normal ground jumps.
            if (!Player.IsWallJumping)
                Player.MovementComponent.Jump(Player.jumpForce);
        }

        public override void Exit() { }

        public override void LogicUpdate()
        {
            if (Player.DashPressed && Player.CanDash)
            {
                Fsm.ChangeState(Player.DashState);
                return;
            }

            // Only allow wall-slide transition after the lockout has expired,
            // preventing immediate re-attachment to the same wall.
            if (Player.IsOnWall && !Player.IsWallJumping)
            {
                Fsm.ChangeState(Player.WallState);
                return;
            }

            // Variable jump height: cut upward momentum when the button is released.
            if (Player.JumpReleased && Player.MovementComponent.Rigidbody.linearVelocity.y > 0f)
            {
                Player.MovementComponent.SetVelocityY(
                    Player.MovementComponent.Rigidbody.linearVelocity.y * 0.5f);
            }

            // Upward momentum exhausted → start falling.
            if (Player.MovementComponent.Rigidbody.linearVelocity.y <= 0f)
            {
                Fsm.ChangeState(Player.FallState);
            }
        }

        public override void PhysicsUpdate() { }

        #endregion
    }
}