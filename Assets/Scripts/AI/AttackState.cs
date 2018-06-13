/**
*   Filename: AttackState.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This script handles the Attack State of the enemy.
*   
**/
using UnityEngine;
using StateMachine;

public class AttackState : State<AI>
{

    private static AttackState _instance;

    private AttackState()
    {
        if (_instance != null) return;

        _instance = this;
    }

    public static AttackState Instance
    {
        get
        {
            if (_instance == null) new AttackState();

            return _instance;
        }
    }

    /**
     * Called when entering the state.
     * Starts the attack animation.
     * 
     * @ _owner the AI it belongs to
     */
    public override void EnterState(AI _owner)
    {
        _owner._animator.SetBool(Animator.StringToHash("Attack"), true);
        Debug.Log("Entering AttackState");
    }

    /**
    * Called when exiting the state.
    * Stops the attack animation.
    * 
    * @ _owner the AI it belongs to
    */
    public override void ExitState(AI _owner)
    {
        _owner._animator.SetBool(Animator.StringToHash("Attack"), false);
        Debug.Log("Exiting AttackState");
    }

    /**
    * Called when updating the state.
    * Check if the player is detected and in melee range.
    * If detected but not in melee range, it changes state to CaseState.
    * If not detected anymore, it changes state to IdleState.
    * Else call the attack() method of the _owner.
    * 
    * @ _owner the AI it belongs to
    */
    public override void UpdateState(AI _owner)
    {

        if (!_owner.melee() && _owner.detect())
        {
            _owner.stateMachine.changeState(ChaseState.Instance);
            
        }
        else if (!_owner.detect())
        {
            _owner.stateMachine.changeState(IdleState.Instance);
        }
        else
        {
            _owner.attack();
        }
    }
}
