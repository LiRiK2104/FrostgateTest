using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Services.DataSaving;
using Cysharp.Threading.Tasks;
using Helpers;
using UnityEngine;
using Zenject;

namespace Core.Services
{
    [RequireComponent(typeof(TargetsLocator))]
    public class MovementQueue : MonoBehaviour
    {
        private const float MinDistanceToTarget = 1.5f;
        private const string TargetsKey = "MOVEMENT_TARGETS";
        
        [SerializeField, Min(0)] private int maxTargetsCount = 5;
        
        private TargetsLocator _targetsLocator;

        private Storage _storage;
        
        private IMovable _movable;
        private IEnumerator _movement;
        private Vector3 _currentTarget;
        
        private Queue<Vector3> _targets = new();
        
        internal event Action<int> Initialized;
        internal event Action TargetEnqueued;
        internal event Action LastTargetReached;

        internal int TargetsCount => _targets.Count;
        

        [Inject]
        private void Construct(Storage storage)
        {
            _storage = storage;
        }
        
        private void Awake()
        {
            _targetsLocator = GetComponent<TargetsLocator>();
            _movable = GetComponent<IMovable>();
        }

        private void OnEnable()
        {
            _targetsLocator.TargetFound += AddTarget;
            _movable.Warped += SetDestination;
        }
        
        private void OnDisable()
        {
            _targetsLocator.TargetFound -= AddTarget;
            _movable.Warped -= SetDestination;
        }

        private void Start()
        {
            LoadAndStart();
        }


        internal void AddTarget(Vector3 position)
        {
            if (_targets.Count >= maxTargetsCount) 
                return;
            
            _targets.Enqueue(position);
            
            Save();
            
            TargetEnqueued?.Invoke();
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
                _currentTarget = _targets.Dequeue();

                Save();
                SetDestination();

                yield return new WaitUntil(() => TargetIsReached(_currentTarget) || 
                                                 _targetsLocator.CanReachPosition(_currentTarget) == false);
            }

            LastTargetReached?.Invoke();
            _movement = null;
        }

        private void SetDestination()
        {
            if (_currentTarget == Vector3.zero) 
                return;
            
            _movable.Agent.SetDestination(_currentTarget);
        }
        
        private bool TargetIsReached(Vector3 targetPosition)
        {
            float distanceToTarget = Vector3.Distance(_movable.Agent.nextPosition, targetPosition);

            return distanceToTarget < MinDistanceToTarget;
        }

        private async void LoadAndStart()
        {
            if (_targets.Count == 0) 
                await Load();

            Initialized?.Invoke(_targets.Count);
        }

        private async UniTask Load()
        {
            Vector3Serializable[] loadedTargets = await _storage.SavingBehaviour.Load(TargetsKey, Array.Empty<Vector3Serializable>());
            
            List<Vector3> convertedTargets = loadedTargets.Select(target => target.ToVector3()).ToList();

            _targets = new Queue<Vector3>(convertedTargets);
        }
        
        private async void Save()
        {
            Vector3Serializable[] savingTargets = _targets.Select(target => new Vector3Serializable(target)).ToArray();
            
            if (_currentTarget != Vector3.zero)
            {
                savingTargets = savingTargets.Append(new Vector3Serializable(_currentTarget)).ToArray();
            }
            
            await _storage.SavingBehaviour.Save(TargetsKey, savingTargets);
        }
    }
}
