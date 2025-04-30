using System;
using System.Collections.Generic;
using UnityEngine;


public class ClientsManager : MonoBehaviour
{
    [SerializeField, WorldVector] private Vector3 _defaultStartPos;
    [SerializeField, WorldVector] private Vector3 _defaultEndPos;
    [SerializeField] private GameobjectPooler _clientsPooler;
    [SerializeField] private ScenePoints _startPositions;
    [SerializeField] private MinMax _instantiateDeltaRange;
    private List<PlaceToGo> _placesToGo = new();
    private float _instantiateDelta;

    void Awake()
    {
        Debug.Log("ClientsManager  Awake");
        PlaceToGo.OnPlaceInitialized += AddPlaceToGo;
        NPCClient.OnCycleFinished += OnClientFinishedCycle;
    }

    private void Start()
    {
        Debug.Log("ClientsManager  Start");
        Init();
        _instantiateDelta = _instantiateDeltaRange.GetRandomValue();
    }


    private void OnDestroy()
    {
        PlaceToGo.OnPlaceInitialized -= AddPlaceToGo;
        NPCClient.OnCycleFinished -= OnClientFinishedCycle;
    }

    private void Update()
    {
        if(_instantiateDelta > 0)
        {
            _instantiateDelta -= Time.deltaTime;
        }
        else
        {
            if(_clientsPooler.TryGetInstance(out GameObject clientObj))
            {
                clientObj.transform.position = _defaultStartPos;
                if(clientObj.TryGetComponent(out NPCClient client))
                {
                    StartClientCycle(client);
                    _instantiateDelta = _instantiateDeltaRange.GetRandomValue();
                }
            }
        }
    }


    private void Init()
    {
        foreach(GameObject clientObj in _clientsPooler)
        {
            if(clientObj.TryGetComponent(out NPCClient client))
            {
                client.Init(_placesToGo, _defaultEndPos);
            }
        }

        for (int i = 0; i < _startPositions.Points.Length; i++)
        {
            SetClient(_startPositions.Points[i]);
        }
    }

    private void AddPlaceToGo(PlaceToGo data)
    {
        _placesToGo.Add(data);
    }


    private void SetClient(Vector3 pos)
    {
        if(_clientsPooler.TryGetInstance(out GameObject clientObj))
        {
            clientObj.transform.position = pos;
            if(clientObj.TryGetComponent(out NPCClient client))
            {
                StartClientCycle(client);
            }
        }
        else
        {
            Debug.LogError("No available clients in the pool.");
        }
    }

    private void StartClientCycle(NPCClient client)
    {
        client.gameObject.SetActive(true);
        client.StartCycle();
    }

    private void OnClientFinishedCycle(NPCClient client)
    {
        client.gameObject.SetActive(false);
        _clientsPooler.ReturnInstance(client.gameObject);
    }
}