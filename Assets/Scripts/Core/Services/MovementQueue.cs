using System;
using System.Collections;
using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;
using Zenject;

namespace Core.Services
{
    [RequireComponent(typeof(TargetsLocator))]
    public class MovementQueue : MonoBehaviour
    {
        private const float MinDistanceToTarget = 1.5f;
        
        [SerializeField, Min(0)] private int maxTargetsCount = 5;
        
        private TargetsLocator _targetsLocator;
        
        private IMovable _movable;
        private IEnumerator _movement;
        
        private readonly Queue<Vector3> _targets = new();
        
        internal event Action<int> TargetsCountChanged;
        internal event Action TargetEnqueued;
        internal event Action LastTargetReached;

        
        private void Awake()
        {
            _targetsLocator = GetComponent<TargetsLocator>();
            _movable = GetComponent<IMovable>();
        }
        
        private void OnEnable()
        {
            _targetsLocator.TargetFound += AddTarget;
        }
        
        private void OnDisable()
        {
            _targetsLocator.TargetFound -= AddTarget;
        }


        internal void AddTarget(Vector3 position)
        {
            if (_targets.Count >= maxTargetsCount) 
                return;
            
            _targets.Enqueue(position);
            
            TargetEnqueued?.Invoke();
            
            InvokeTargetsCountChanged();
        }
        
        internal void StartMovement()
        {
            if (_movement != null) 
                return;

            _movement = Move();
            StartCoroutine(Move());
        }
        
        internal void StopMovement()
        {
            if (_movement == null) 
                return;

            StopCoroutine(_movement);
            _movement = null;
        }

        private IEnumerator Move()
        {
            while (_targets.Count > 0)
            {
                Vector3 targetPosition = _targets.Dequeue();
                
                InvokeTargetsCountChanged();
                    
                _movable.Agent.SetDestination(targetPosition);

                yield return new WaitUntil(() => TargetIsReached(targetPosition));
            }

            LastTargetReached?.Invoke();
            _movement = null;
        }
        
        private bool TargetIsReached(Vector3 targetPosition)
        {
            float distanceToTarget = Vector3.Distance(_movable.Agent.nextPosition, targetPosition);

            return distanceToTarget < MinDistanceToTarget;
        }

        private void InvokeTargetsCountChanged()
        {
            TargetsCountChanged?.Invoke(_targets.Count);
        }
    }
}
