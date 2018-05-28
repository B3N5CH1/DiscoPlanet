using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

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

    public override void EnterState(AI _owner)
    {

        _owner.attack();
        Debug.Log("Entering AttackState");
    }

    public override void ExitState(AI _owner)
    {

        Debug.Log("Exiting AttackState");
    }

    public override void UpdateState(AI _owner)
    {

        if (!_owner.melee() && _owner.detect())
        {
            _owner.stateMachine.changeState(ChaseState.Instance);
            
        }
        else if (_owner.melee() && _owner.detect())
        {
            _owner.stateMachine.changeState(AttackState.Instance);
        }
        else
        {
            _owner.stateMachine.changeState(IdleState.Instance);
        }
    }
}
