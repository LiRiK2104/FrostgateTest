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

            _movementQueue.Initialized += TryTransit;
            _movementQueue.TargetEnqueued += SetNeedTransit;
            
            TryTransit(_movementQueue.TargetsCount);
        }
        
        private void OnDisable()
        {
            _movementQueue.Initialized -= TryTransit;
            _movementQueue.TargetEnqueued -= SetNeedTransit;
        }


        private void TryTransit(int targetsCount)
        {
            if (targetsCount > 0) 
                SetNeedTransit();
        }
    }
}
