
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentPrioritySeter : MonoBehaviour
{
    private static int _priority = 0;

    private void Awake()
    {
        GetComponent<NavMeshAgent>().avoidancePriority = _priority++;
    }
}