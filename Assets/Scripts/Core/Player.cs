using Core.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Player : MonoBehaviour, IMovable
    {
        [SerializeField, Min(0)] private float speed = 3.5f;
        [SerializeField, Min(0)] private float angularSpeed = 120f;

        public NavMeshAgent Agent { get; private set; }


        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            Agent.speed = speed;
            Agent.angularSpeed = angularSpeed;
        }
    }
}
