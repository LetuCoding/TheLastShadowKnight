using System;
using Player.StateMachine;
using Player.StateMachine.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Player : MonoBehaviour
    {

        #region StateMachine & States
        
        
        //State Machine
        private PlayerStateMachine fsm;
        
        //States
        private IdleState idleState;
        

        #endregion

        public InputAction playerInput;
        
        
        
        

        private void Awake()
        {
            playerInput = new InputAction();
            fsm = new PlayerStateMachine();
            idleState = new IdleState(fsm, this);
            
        }


        private void Start()
        {
            fsm.Initialize(idleState);
        }

        private void Update()
        {
            
        }
        
        #region Getters and Setters

        
        
        
        
        #endregion
        
        
        
        
        
        
        
    }
    
}