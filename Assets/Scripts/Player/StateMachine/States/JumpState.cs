using Unity.VisualScripting;
using UnityEngine;

namespace Player.StateMachine.States
{
    public class JumpState : PlayerState
    {
        public JumpState(PlayerStateMachine fsm, Player player, InputSystem_Actions inputActions) : base(fsm, player, inputActions)
        {
        }

        public override void Enter()
        {
            
            Movement.Jump(8);
            
        }

        public override void Exit()
        {
            
        }

        public override void LogicUpdate()
        {
            if (Player.isGrounded)
            { 
                Debug.Log("Changing to idle state");
                Fsm.ChangeState(Player.idleState);
            }
            
        }

        public override void PhysicsUpdate()
        {
            
        }

    }
}