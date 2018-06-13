/**
*   Filename: StateMachine.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This script creates a namespace so we can acces its content easier in other scripts.
*       It creates the State Machine, which is responsible for handling our AI states.
*       And creates the abstract class State. 
*   
**/

namespace StateMachine
{
    public class StateMachine<T>
    {
        public State<T> currentState { get; private set; }
        public T Owner;

        /**
         * State Machine constructor
         * 
         * @_o The owner of this state machine
         */
        public StateMachine (T _o)
        {
            Owner = _o;
            currentState = null;
        }

        /**
         * Handle the change of states.
         * 
         * @_newState the state to move in
         */
        public void changeState (State<T> _newState)
        {
            // If the current state is null (when we start the fsm) skips the first state.exitstate
            if (currentState != null) currentState.ExitState(Owner);
            currentState = _newState;
            currentState.EnterState(Owner);
        }

        // On State Machine update, call the update method from the right state
        public void Update()
        {

            if (currentState != null) currentState.UpdateState(Owner);
        }
    }

    // The abstract State class
    public abstract class State<T>
    {

        public abstract void EnterState(T _owner);
        public abstract void ExitState(T _owner);
        public abstract void UpdateState(T _owner);
    }
}
