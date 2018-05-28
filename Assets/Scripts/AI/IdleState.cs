using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

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

    public override void EnterState(AI _owner)
    {

        Debug.Log("Entering IdleState");
    }

    public override void ExitState(AI _owner)
    {

        Debug.Log("Exiting IdleState");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.detect() && ! _owner.melee())
        {
            _owner.stateMachine.changeState(ChaseState.Instance);
        }
        if (_owner.detect() && _owner.melee())
        {
            _owner.stateMachine.changeState(AttackState.Instance);
        }


    }
}
