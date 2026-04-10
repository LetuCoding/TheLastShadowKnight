using Player;
using Player.StateMachine;
using UnityEngine;
using Zenject;


public abstract class PlayerState
{
    protected PlayerStateMachine Fsm;
    protected Player.Player Player;
    protected MovementComponent Movement;
    
    
    protected InputSystem_Actions _inputActions;

    public PlayerState(PlayerStateMachine fsm, Player.Player  player, InputSystem_Actions inputActions)
    {
        this.Fsm = fsm;
        this.Player = player;
        _inputActions = inputActions;
        Movement = player.MovementComponent;
    }

    
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate()
    {
        
    }    

}