using UnityEngine;

namespace Core.StateMachineSystem
{
    public class ExitTimeTransition : Transition
    {
        [SerializeField] private State exitTimeState;

        protected override void OnEnable()
        {
            base.OnEnable();

            exitTimeState.OnExitTime += SetNeedTransit;
        }

        private void OnDisable()
        {
            exitTimeState.OnExitTime -= SetNeedTransit;
        }
    }
}
