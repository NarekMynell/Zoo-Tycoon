using UnityEngine;
using UnityEngine.AI;

namespace States
{
    public class NPCCharacterWalk : NPCMove
    {
        private NavMeshAgent _navMeshAgent;
        private bool _isArrived;
        public Vector3 destination;
        public bool IsArrived => _isArrived;


        public NPCCharacterWalk(Animator animator, NavMeshAgent navMeshAgent) : base(animator, "Walk")
        {
            _navMeshAgent = navMeshAgent;
        }

        public override void Enter()
        {
            base.Enter();
            _isArrived = false;

            if (!_navMeshAgent.isOnNavMesh)
            {

                if (NavMesh.SamplePosition(_navMeshAgent.transform.position, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
                {
                    _navMeshAgent.Warp(hit.position);
                }
                else
                {
                    Debug.LogError("The NavMesh has not been found");
                    _isArrived = true;
                }
            }

            // //Vector3 destination = _meshFilter.GetRandomPointOnSurface();
            // Vector3 destination = _navMeshAgent.transform.parent.position + Random.insideUnitSphere * _areaRadius;
            // destination.y = _navMeshAgent.transform.position.y;
            _navMeshAgent.SetDestination(destination);
        }

        public override void Update()
        {
            base.Update();

            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);

            if (HasReachedDestination()) _isArrived = true;
            if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) _isArrived = true;

            // if(UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            // {
            //     //Debug.Log("Left mouse button clicked");
            //     // Get the mouse position in world space
            //     Ray ray = Camera.main.ScreenPointToRay(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
            //     if (Physics.Raycast(ray, out RaycastHit hit))
            //     {
            //         // Set the destination of the NavMeshAgent to the clicked position
            //         _navMeshAgent.SetDestination(hit.point);
            //     }
            // }
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