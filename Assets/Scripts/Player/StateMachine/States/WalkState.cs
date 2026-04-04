using UnityEngine;

namespace Player.StateMachine.States
{
    public class WalkState : PlayerState
    {
        public WalkState(PlayerStateMachine fsm, Player player) : base(fsm, player)
        {
           
        }
        
        public override void Enter()
        {
            Debug.Log("Entering WalkState");
        }

        public override void Exit()
        {
            Debug.Log("Exiting WalkState");
        }

        public override void LogicUpdate()
        {
            
        }



        
        
        
        
        
        
        
    }
} 