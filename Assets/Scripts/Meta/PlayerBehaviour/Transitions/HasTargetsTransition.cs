using Core.Services;
using Core.StateMachineSystem;

namespace Meta.PlayerBehaviour.Transitions
{
    public class HasTargetsTransition : Transition
    {
        private MovementQueue _movementQueue;
        
        
        private void Awake()
        {
            _movementQueue = GetComponentInParent<MovementQueue>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        
            _movementQueue.TargetEnqueued += SetNeedTransit;
        }
        
        private void OnDisable()
        {
            _movementQueue.TargetEnqueued -= SetNeedTransit;
        }
    }
}
