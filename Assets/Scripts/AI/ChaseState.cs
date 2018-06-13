/**
*   Filename: ChaseState.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This script handles the chase state of the enemies.
*   
**/
using UnityEngine;
using StateMachine;

public class ChaseState : State<AI>
{

    private static ChaseState _instance;

    private ChaseState()
    {
        if (_instance != null) return;

        _instance = this;
    }

    public static ChaseState Instance
    {
        get
        {
            if (_instance == null) new ChaseState();

            return _instance;
        }
    }

    /**
     * Called when entering the state.
     * Start the walk animation.
     * 
     * @ _owner the AI it belongs to
     */
    public override void EnterState(AI _owner)
    {

        _owner._animator.SetBool(Animator.StringToHash("Walk"), true);
        Debug.Log("Entering ChaseState");
    }

    /**
    * Called when exiting the state.
    * Stops the walk animation.
    * 
    * @ _owner the AI it belongs to
    */
    public override void ExitState(AI _owner)
    {

        _owner._animator.SetBool(Animator.StringToHash("Walk"), false);
        Debug.Log("Exiting ChaseState");
    }

    /**
    * Called when updating the state.
    * Check if the player is detected and in melee range.
    * If detected but not in melee range, call the chase() method of the _owner.
    * If detected and in melee range, it changes state to AttackState.
    * If not detected anymore, it changes state to IdleState.
    * 
    * @ _owner the AI it belongs to
    */
    public override void UpdateState(AI _owner)
    {

        if (!_owner.detect())
        {
            _owner.stateMachine.changeState(IdleState.Instance);
        }
        else if (_owner.detect() && _owner.melee())
        {
            _owner.stateMachine.changeState(AttackState.Instance);
        }
        else if (_owner.detect() && !_owner.melee())
        {
            _owner.chase();
        }
    }
}
