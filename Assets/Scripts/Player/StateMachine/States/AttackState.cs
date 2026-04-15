using UnityEngine;

namespace Player.StateMachine.States
{
    public class AttackState : PlayerState
    {
        public bool IsAttacking;
        private float _lastTimeAttacked;
        readonly float _comboWindow = 0.3f;
        private float _attackDir;
        private float _inputBuffer;
        public AttackState(PlayerStateMachine fsm, Player player, InputSystem_Actions inputActions) : base(fsm, player, inputActions)
        {
          
        }

        public override void Enter()
        {

            _attackDir = Player.LastMoveInput;
            Attack();
            IsAttacking = true;
            _lastTimeAttacked = Time.time;
        }




        public override void LogicUpdate()
        {
            if (Time.time - _lastTimeAttacked > _comboWindow)
            {
                Fsm.ChangeState(Player.IdleState);
                return;
            }

            if (Player.AttackPressed)
            {
                _lastTimeAttacked = Time.time;
                _inputBuffer++;

            }

            if (_inputBuffer > 0 && Time.time - _lastTimeAttacked  > _comboWindow - 0.2)
            {
                Attack();
            }
            
        }

        public override void Exit()
        {
            IsAttacking = false;
        }

        public void Attack()
        {
            Debug.Log(Player.LastMoveInput);
            
            
            if (Player.IsGrounded)
            {
                
                Player.MovementComponent.Rigidbody.linearVelocity = new Vector2(
                    5f * _attackDir,
                    Player.MovementComponent.Rigidbody.linearVelocity.y
                );
            }
          
            
        }
        
        
        
    }
}