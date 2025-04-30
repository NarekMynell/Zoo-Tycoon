using System;
using UnityEngine;

public class PlaceToGo : MonoBehaviour, IPlaceToGo
{
        [SerializeField] private MinMax _stayDuration;
        [SerializeField, LocalVector] protected Vector3 _offset;
        [SerializeField] private bool _isStrollable;
        [SerializeField] private float _radius;

        public MinMax StayDuration => _stayDuration;
        public Vector3 Pos => transform.position + _offset;
        public bool IsStrollable => _isStrollable;
        public float Radius => _radius;

        public static event Action<PlaceToGo> OnPlaceInitialized;


    protected virtual void Start()
    {
        Debug.Log("PlaceToGo  Start");
        SendPlace();
    }

    public void SendPlace()
    {
        Debug.Log(gameObject.name);
        if(OnPlaceInitialized == null) Debug.Log("null");
        OnPlaceInitialized?.Invoke(this);
    }
}