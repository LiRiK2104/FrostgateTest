using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.StateMachineSystem
{
    public abstract class State : MonoBehaviour
    {
        [SerializeField] private List<Transition> transitions = new();

        internal event Action OnExitTime;
        

        internal void Enter()
        {
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
                transitions.ForEach(transition => transition.gameObject.SetActive(true));
            }   
        }

        internal void Exit()
        {
            if (gameObject.activeSelf)
            {
                transitions.ForEach(transition => transition.gameObject.SetActive(false));
                gameObject.SetActive(false);
            } 
        }

        internal State GetNextState()
        {
            foreach (var transition in transitions)
            {
                if (transition.NeedTransit)
                    return transition.TargetState;
            }

            return null;
        }

        protected void NotifyExitTime()
        {
            OnExitTime?.Invoke();
        }
    }
}
