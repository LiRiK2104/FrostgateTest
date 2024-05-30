using Core.Services;
using Core.StateMachineSystem;

namespace Meta.PlayerBehaviour.States
{
    public class MovementState : State
    {
        private MovementQueue _movementQueue;
        
        
        private void Awake()
        {
            _movementQueue = GetComponentInParent<MovementQueue>();
        }

        private void OnEnable()
        {
            _movementQueue.StartMovement();
        }
        
        private void OnDisable()
        {
            _movementQueue.StopMovement();
        }
    }
}