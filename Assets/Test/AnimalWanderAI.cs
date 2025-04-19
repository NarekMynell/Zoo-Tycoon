using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AnimalWanderAI : MonoBehaviour
{
    [SerializeField] private MeshFilter _wanderAreaMeshFilter;
    [SerializeField] private MeshFilter _meshFilter;
    public Transform _ponit;
    public bool _isCorrect;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 randomPoint = _meshFilter.GetRandomPointOnSurface(_isCorrect);
            Debug.Log("Random Point: " + randomPoint);

            // Ստեղծել գնդիկ պատահական կետում (վիզուալ ստուգման համար)
            _ponit.position = randomPoint;
        }

        HandleMovement();
        UpdateAnimator();
    }





    [Header("Wander Settings")]
    public MeshRenderer cageFloor; // Assign in inspector
    public float moveSpeed = 3.5f;
    public float runSpeed = 6f;
    public float rotationSpeed = 5f;
    public float idleTimeMin = 2f;
    public float idleTimeMax = 5f;
    public float destinationRadius = 0.5f;

    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 cageMinBounds;
    private Vector3 cageMaxBounds;

    private float nextActionTime;
    private enum State { Idle, Moving }
    private State currentState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        CalculateBounds();

        SwitchToIdle();
    }


    private void CalculateBounds()
    {
        if (cageFloor == null)
        {
            Debug.LogError("Cage Floor not assigned.");
            enabled = false;
            return;
        }

        Bounds bounds = cageFloor.bounds;
        cageMinBounds = bounds.min;
        cageMaxBounds = bounds.max;
    }

    private void HandleMovement()
    {
        switch (currentState)
        {
            case State.Idle:
                if (Time.time >= nextActionTime)
                {
                    TrySetNewDestination();
                }
                break;

            case State.Moving:
                if (!agent.pathPending && agent.remainingDistance <= destinationRadius)
                {
                    SwitchToIdle();
                }
                break;
        }
    }

    private void TrySetNewDestination()
    {
        for (int i = 0; i < 10; i++) // Try 10 times max
        {
            Vector3 randomPoint = _wanderAreaMeshFilter.GetRandomPointOnSurface();
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    float speed = Random.value < 0.5f ? moveSpeed : runSpeed;
                    agent.speed = speed;
                    agent.SetDestination(hit.position);
                    currentState = State.Moving;
                    return;
                }
            }
        }

        // Couldn't find a valid path
        SwitchToIdle();
    }

    private void SwitchToIdle()
    {
        agent.ResetPath();
        nextActionTime = Time.time + Random.Range(idleTimeMin, idleTimeMax);
        currentState = State.Idle;
    }

    private void UpdateAnimator()
    {
        float velocity = agent.velocity.magnitude;
        animator.SetFloat("Speed", velocity);
        
        // Optional: Smoothly rotate toward movement direction
        if (velocity > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
