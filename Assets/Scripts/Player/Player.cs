using System;
using Player.StateMachine;
using Player.StateMachine.States;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Player : MonoBehaviour
    {

        #region Variables
        
        [Header("References")]
        
        public float groundCheckDistance;
        public Transform groundCheck;
        public LayerMask groundLayer;
        
        
        
        public bool isGrounded {get; private set;}
        public bool jumpPressed;
        public bool jumpReleased;
        
        
        
        #endregion
        
        
        #region Monobehaviors
        
        public MovementComponent movement;
        
        
        #endregion
        
        
        #region StateMachine & States

        private PlayerStateMachine fsm;
        public IdleState idleState;
        public JumpState jumpState;
        #endregion


        #region Injectables
        
        [Inject]
        private InputSystem_Actions _inputActions;
        
        
        #endregion
        

        private void Awake()
        {
            fsm = this.AddComponent<PlayerStateMachine>();
            
        }
        
        private void Start()
        {
            //Components
            movement = GetComponent<MovementComponent>();
            
            idleState = new IdleState(fsm, this,  _inputActions);
            jumpState = new JumpState(fsm, this,  _inputActions);
            
            
            
            
            
            _inputActions.Enable();
            fsm.Initialize(idleState);
        }

        private void Update()
        {
            
            
            
            jumpPressed = _inputActions.Player.Jump.WasPressedThisFrame();
            jumpReleased = _inputActions.Player.Jump.WasReleasedThisFrame();
            
            
            
            
            Debug.Log(fsm.CurrentState);
            fsm.CurrentState.LogicUpdate();
            
            
            CheckGrounded();
        }


        #region PlayerChecks
        
        
        //Method that checks wether the player is touching the ground or not
        private void CheckGrounded()
        {
            if (Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, groundLayer))
            {
                isGrounded = true;
                return;
            }
            isGrounded = false;
        }


        #endregion

        
        
        //We draw gizmos on editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        }
       
        
        private void OnDisable()
        {
            if (_inputActions != null)
                _inputActions.Disable();
        }
    }
}