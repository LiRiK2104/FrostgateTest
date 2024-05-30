using System;
using Core.Services;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class TargetsLocator : MonoBehaviour
    {
        [SerializeField] private LayerMask groundMask;

        private NavMeshAgent _agent;
        private InputListener _inputListener;

        internal event Action<Vector3> TargetFound;

    
        [Inject]
        private void Construct(InputListener inputListener)
        {
            _inputListener = inputListener;
        }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            _inputListener.PointerDown += OnPointerDown;
        }
    
        private void OnDisable()
        {
            _inputListener.PointerDown -= OnPointerDown;
        }


        internal bool CanReachPosition(Vector3 position)
        {
            NavMeshPath path = new();
            _agent.CalculatePath(position, path);
        
            return path.status == NavMeshPathStatus.PathComplete;
        }
        
        private void OnPointerDown(Vector2 screenPosition)
        {
            if (TryGetReachablePosition(screenPosition, out Vector3 targetPosition) == false) 
                return;

            TargetFound?.Invoke(targetPosition);
        }

        private bool TryGetReachablePosition(Vector2 screenPosition, out Vector3 targetPosition)
        {
            const float maxRayDistance = 1000f;
     
            targetPosition = Vector3.zero;

            if (Camera.main == null) 
                return false;
        
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        
            if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, groundMask.value) == false) 
                return false;

            if (CanReachPosition(hit.point) == false) 
                return false;

            targetPosition = hit.point;
        
            return true;
        }
    }
}
