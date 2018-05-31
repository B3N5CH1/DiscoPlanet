using UnityEngine;
using StateMachine;

/*
 * This script handle the Idle state.
 */

public class IdleState : State<AI>
{

    private static IdleState _instance;

    private IdleState()
    {
        if (_instance != null) return;

        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (_instance == null) new IdleState();

            return _instance;
        }
    }

    /**
     * Called when entering the state.
     * 
     * @ _owner the AI it belongs to
     */
    public override void EnterState(AI _owner)
    {

        Debug.Log("Entering IdleState");
    }

    /**
     * Called when exiting the state.
     * 
     * @ _owner the AI it belongs to
     */
    public override void ExitState(AI _owner)
    {

        Debug.Log("Exiting IdleState");
    }

    /**
    * Called when updating the state.
    * Check if the player is detected and in melee range.
    * If detected but not in melee range, it changes state to CaseState.
    * If detected and in melee range, it changes state to AttackState.
    * Else it stays in IdleState.
    * 
    * @ _owner the AI it belongs to
    */
    public override void UpdateState(AI _owner)
    {
        if (_owner.detect() && !_owner.melee())
        {
            _owner.stateMachine.changeState(ChaseState.Instance);
        }
        if (_owner.detect() && _owner.melee())
        {
            _owner.stateMachine.changeState(AttackState.Instance);
        }


    }
}
