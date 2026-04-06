using UnityEngine;
using UnityEngine.Lumin;

namespace Player.StateMachine.States
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerStateMachine fsm, Player player, InputSystem_Actions inputActions) 
            : base(fsm, player, inputActions) { }
        
        
        public override void Enter()
        {
            
        }

        public override void Exit()
        {
          
        }

        public override void LogicUpdate()
        {
          
            
            //if move pressed, change state
            if (Player.jumpPressed)
            {
               Fsm.ChangeState(Player.jumpState);
                
            }
            
        }
    }
}