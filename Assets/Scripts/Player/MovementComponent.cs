using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Player
{
    public class MovementComponent : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        
        
        [Inject]
        private InputSystem_Actions _inputActions;
        
        
        private InputAction _move;
        
       
        
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _move = _inputActions.Player.Move;
        }

        void FixedUpdate()
        {
            Vector2 movement = _move.ReadValue<Vector2>();
            _rigidbody.linearVelocity = new Vector2(movement.x * 2, _rigidbody.linearVelocity.y);
        }


        public void Jump(float jumpForce)
        {
            Debug.Log("Jump");
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0); 
            
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);
        }
    }
}