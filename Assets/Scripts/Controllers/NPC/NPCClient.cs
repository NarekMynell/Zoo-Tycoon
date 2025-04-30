using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlaceToGo;


public class NPCClient : MonoBehaviour, ICharacter
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public static event Action<NPCClient> OnCycleFinished;

    private States.StateMachine _stateMachine;
    private States.NPCCharacterIdle _characterIdle;
    private States.NPCCharacterWalk _characterWalk;
    private List<PlaceToGo> _fullPlacesToGo;
    private Vector3 _endPos;
    private List<PlaceToGo> _currentPlacesToGo;


    void Awake()
    {
        _stateMachine = new States.StateMachine();
        _characterWalk = new(_animator, _navMeshAgent);
        _characterIdle = new(_animator);
    }

    void Update()
    {
        _stateMachine.CurrentState?.Update();
    }

    public void Init(List<PlaceToGo> placesToGo, Vector3 endPos)
    {
        _fullPlacesToGo = placesToGo;
        _endPos = endPos;
    }

    public void StartCycle()
    {
        _currentPlacesToGo = _fullPlacesToGo.GetRandomItems(UnityEngine.Random.Range(1, _fullPlacesToGo.Count));
        StartCoroutine(Cycle());
    }

    private void FinishCycle()
    {
        _currentPlacesToGo = null;
        OnCycleFinished?.Invoke(this);
    }

    private IEnumerator Cycle()
    {
        foreach (PlaceToGo placeToGo in _currentPlacesToGo)
        {
            Vector3 pos = placeToGo.Pos;
            Vector3 offset = UnityEngine.Random.insideUnitSphere * placeToGo.Radius;
            offset.y = 0;
            pos += offset;
            GoTo(pos);
            yield return new WaitUntil(() => _characterWalk.IsArrived);

            if(placeToGo.IsStrollable)
            {
                float fullDur = placeToGo.StayDuration.GetRandomValue();
                int partsCount = UnityEngine.Random.Range(1, 5);
                float[] durations = fullDur.GetRandomParts(partsCount);

                for (int i = 0; i < partsCount; i++)
                {
                    float dur = durations[i];
                    Idle();
                    yield return new WaitForSeconds(dur);

                    Vector3 radomPos = placeToGo.Pos + UnityEngine.Random.insideUnitSphere * placeToGo.Radius;
                    radomPos.y = placeToGo.Pos.y;
                    pos = placeToGo.Pos;
                    offset = UnityEngine.Random.insideUnitSphere * placeToGo.Radius;
                    offset.y = 0;
                    pos += offset;
                    GoTo(pos);
                    yield return new WaitUntil(() => _characterWalk.IsArrived);
                }
            }
            else
            {
                float dur =  placeToGo.StayDuration.GetRandomValue();
                Idle();
                yield return new WaitForSeconds(dur);
            }
        }

        GoTo(_endPos);
        yield return new WaitUntil(() => _characterWalk.IsArrived);
        FinishCycle();
    }

    public void Idle()
    {
        _stateMachine.ChangeState(_characterIdle);
    }

    public void GoTo(Vector3 pos)
    {
        _characterWalk.destination = pos;
        Walk();
    }

    public void Walk()
    {
        _stateMachine.ChangeState(_characterWalk);
    }
}