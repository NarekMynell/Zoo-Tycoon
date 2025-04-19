using System;
using UnityEngine;
using UnityEngine.AI;


public class NPCAnimal : MonoBehaviour, IAnimal
{
    [SerializeField] private Scriptables.NPCStates _npcStates;
    [SerializeField] private Animator _animator;
    [SerializeField] private MeshFilter _cage;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _runSpeed = 4f;
    private States.StateMachine _stateMachine;
    private States.NPCAnimalIdle _animalIdle;
    private States.NPCAnimalEat _animalEat;
    private States.NPCAnimalMove _animalMove;


    void Awake()
    {
        _stateMachine = new States.StateMachine();
        _animalIdle = new(_animator, SetRandomState);
        _animalEat = new(_animator, SetRandomState);
        _animalMove = new(_animator, "Move", _navMeshAgent, _cage, SetRandomState);
    }

    void Start()
    {
        SetRandomState();
    }

    void Update()
    {
        _stateMachine.CurrentState.Update();
    }


    private void SetRandomState()
    {
        NPCState state = _npcStates.GetRandomState();
        switch (state)
        {
            case NPCState.Walk:
                Walk();
                break;
            case NPCState.Run:
                Run();
                break;
            case NPCState.Eat:
                Eat();
                break;
            case NPCState.Swim:
                Swim();
                break;
            default:
                Idle();
                break;
        }
    }

    public void Idle()
    {
        _stateMachine.ChangeState(_animalIdle);
    }

    public void Eat()
    {
        _stateMachine.ChangeState(_animalEat);
    }

    public void Walk()
    {
        _navMeshAgent.speed = _walkSpeed;
        _stateMachine.ChangeState(_animalMove);
    }

    public void Run()
    {
        _navMeshAgent.speed = _runSpeed;
        _stateMachine.ChangeState(_animalMove);
    }

    public void Swim()
    {
    }
}