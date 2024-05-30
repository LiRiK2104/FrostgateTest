using System;
using UnityEngine.AI;

namespace Core.Interfaces
{
    public interface IMovable
    {
        public event Action Warped;
        
        public NavMeshAgent Agent { get; }
    }
}
