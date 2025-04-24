using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace States
{
    public class NPCAnimalMove : NPCMove
    {
        private NavMeshAgent _navMeshAgent;
        private System.Action _finishCallback;
        private float _areaRadius;

        public NPCAnimalMove(Animator animator, string clipName, NavMeshAgent navMeshAgent, float areaRadius, System.Action finishCallback) : base(animator, clipName)
        {
            _navMeshAgent = navMeshAgent;
            _finishCallback = finishCallback;
            _areaRadius = areaRadius;
        }

        public override void Enter()
        {
            base.Enter();

            if (!_navMeshAgent.isOnNavMesh)
            {

                if (NavMesh.SamplePosition(_navMeshAgent.transform.position, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
                {
                    _navMeshAgent.Warp(hit.position);
                }
                else
                {
                    Debug.LogError("The NavMesh has not been found");
                    _finishCallback?.Invoke();
                }
            }

            //Vector3 destination = _meshFilter.GetRandomPointOnSurface();
            Vector3 destination = _navMeshAgent.transform.parent.position + Random.insideUnitSphere * _areaRadius;
            destination.y = _navMeshAgent.transform.position.y;
            _navMeshAgent.SetDestination(destination);
        }

        public override void Update()
        {
            base.Update();

            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);

            if (HasReachedDestination()) _finishCallback?.Invoke();
            if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) _finishCallback?.Invoke();
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