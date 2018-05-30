using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

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

    public override void EnterState(AI _owner)
    {

        _owner._animator.SetBool(Animator.StringToHash("Walk"), true);
        Debug.Log("Entering ChaseState");
    }

    public override void ExitState(AI _owner)
    {

        _owner._animator.SetBool(Animator.StringToHash("Walk"), false);
        Debug.Log("Exiting ChaseState");
    }

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
