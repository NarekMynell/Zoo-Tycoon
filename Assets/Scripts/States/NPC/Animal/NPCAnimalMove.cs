using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace States
{
    public class NPCAnimalMove : NPCMove
    {
        private MeshFilter _meshFilter;
        private NavMeshAgent _navMeshAgent;
        private UnityAction _finishCallback;

        public NPCAnimalMove(Animator animator, string clipName, NavMeshAgent navMeshAgent, MeshFilter meshFilter, UnityAction finishCallback) : base(animator, clipName)
        {
            _navMeshAgent = navMeshAgent;
            _meshFilter = meshFilter;
            _finishCallback = finishCallback;
        }

        public override void Enter()
        {
            base.Enter();

            if (!_navMeshAgent.isOnNavMesh)
            {

                if (NavMesh.SamplePosition(_meshFilter.transform.position, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
                {
                    _navMeshAgent.Warp(hit.position);
                }
                else
                {
                    Debug.LogError("The NavMesh has not been found");
                    _finishCallback?.Invoke();
                }
            }

            Vector3 destination = _meshFilter.GetRandomPointOnSurface();
            _navMeshAgent.SetDestination(destination);
        }

        public override void Update()
        {
            base.Update();

            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
            if (HasReachedDestination()) _finishCallback?.Invoke();
        }
        

        public bool HasReachedDestination()
        {
            if (!_navMeshAgent.pathPending)
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}