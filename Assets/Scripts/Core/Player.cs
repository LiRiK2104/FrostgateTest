using System;
using Core.Interfaces;
using Core.Services;
using Core.Services.DataSaving;
using Helpers;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Player : MonoBehaviour, IMovable
    {
        private const string PositionKey = "PLAYER_POSITION";
        private const string RotationKey = "ROTATION_POSITION";
        
        [SerializeField, Min(0)] private float speed = 3.5f;
        [SerializeField, Min(0)] private float angularSpeed = 120f;

        private Storage _storage;

        public event Action Warped;
        
        public NavMeshAgent Agent { get; private set; }


        [Inject]
        private void Construct(Storage storage)
        {
            _storage = storage;
        }

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }
        
        private void OnDisable()
        {
            Save();
        }

        private void Start()
        {
            Agent.speed = speed;
            Agent.angularSpeed = angularSpeed;
            
            Load();
        }


        private async void Load()
        {
            Vector3Serializable loadedPosition = await _storage.SavingBehaviour.Load(PositionKey, new Vector3Serializable(Vector3.zero));
            QuaternionSerializable loadedRotation = await _storage.SavingBehaviour.Load(RotationKey, new QuaternionSerializable(Quaternion.identity));

            Agent.Warp(loadedPosition.ToVector3());
            transform.rotation = loadedRotation.ToQuaternion();
            
            Warped?.Invoke();
        }
        
        private async void Save()
        {
            if (_storage == null) 
                return;
            
            Vector3Serializable convertedPosition = new(Agent.nextPosition);
            QuaternionSerializable convertedRotation = new(transform.rotation);
            
            await _storage.SavingBehaviour.Save(PositionKey, convertedPosition);
            await _storage.SavingBehaviour.Save(RotationKey, convertedRotation);
        }


        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus == false)
                Save();
        }
    }
}
