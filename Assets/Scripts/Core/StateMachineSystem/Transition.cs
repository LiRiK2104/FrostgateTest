using UnityEngine;

namespace Core.StateMachineSystem
{
    public abstract class Transition : MonoBehaviour
    {
        [SerializeField] private State targetState;

        internal State TargetState => targetState;
        internal bool NeedTransit { get; private set; }

        
        protected virtual void OnEnable()
        {
            NeedTransit = false;
        }


        protected void SetNeedTransit()
        {
            NeedTransit = true;
        }
    }
}
