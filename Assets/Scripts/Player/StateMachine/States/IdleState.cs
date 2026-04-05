using UnityEngine;

namespace Player.StateMachine.States
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerStateMachine fsm, Player player, InputSystem_Actions inputActions) 
            : base(fsm, player, inputActions) { }
        
        
        public override void Enter()
        {
            Debug.Log("Entering IdleState");
        }

        public override void Exit()
        {
            Debug.Log("Exiting IdleState");
        }

        public override void LogicUpdate()
        {
            //if move pressed, change state
            if (Player.jumpPressed)
            {
                Debug.Log("Changing jump");
                Fsm.ChangeState(Player.jumpState);
                
            }
        }
    }
}