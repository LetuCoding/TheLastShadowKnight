using Unity.VisualScripting;
using UnityEngine;

namespace Player.StateMachine.States
{
    public class JumpState : PlayerState
    {
        
        private float _timeInState;
        public JumpState(PlayerStateMachine fsm, Player player, InputSystem_Actions inputActions) : base(fsm, player, inputActions)
        {
        }

        public override void Enter()
        {
            _timeInState = 0f;
            Movement.Jump(8);
            
        }

        public override void Exit()
        {
            
        }

        public override void LogicUpdate()
        {
            _timeInState += Time.deltaTime;

            if (_timeInState > 0.1f && Player.isGrounded)
                Fsm.ChangeState(Player.idleState);
            
        }

        public override void PhysicsUpdate()
        {
            
        }

    }
}