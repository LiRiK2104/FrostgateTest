using UnityEngine.AI;

namespace Core.Interfaces
{
    public interface IMovable
    {
        public NavMeshAgent Agent { get; }
    }
}
