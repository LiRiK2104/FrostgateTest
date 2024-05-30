using UnityEngine;

namespace Core.StateMachineSystem
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private State startState;
        [SerializeField] private AnyState anyState;

        private State _currentState;

        
        private void Start()
        {
            Reset();
        }

        private void Update()
        {
            State nextState;
            
            if (anyState != null)
            {
                nextState = anyState.GetNextState();

                if (nextState != null)
                {
                    Transit(nextState);
                    return;
                }    
            }

            if (_currentState == null) 
                return;

            nextState = _currentState.GetNextState();

            if (nextState != null)
                Transit(nextState);
        }

        
        private void Reset()
        {
            State[] allStates = GetComponentsInChildren<State>();

            foreach (var state in allStates)
            {
                if (anyState == null || state != anyState)
                {
                    state.Exit();   
                }
            }
            
            _currentState = startState;
            
            if (_currentState != null) 
                _currentState.Enter();
        }

        private void Transit(State nextState)
        {
            if (_currentState != null) 
                _currentState.Exit();

            _currentState = nextState;
            
            if (_currentState != null) 
                _currentState.Enter();
        }
    }
}
