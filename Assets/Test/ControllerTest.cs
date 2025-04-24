using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviour
{
    public bool roundOnlyAfterReaching;
    [SerializeField] private double _value;

    void Update()
    {
        Debug.Log(_value.FormatLargeNumber(roundOnlyAfterReaching)) ;
    }
    // [Range(0, 4)] public float _speed; 
    // [Range(0, 1)] public float _state; 
    // [Range(0, 1)] public float _vert;

    // private Animator _animator;


    // private void Start()
    // {
    //     _animator = GetComponent<Animator>();
    // }


    // void Update()
    // {
    //     transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    //     _animator.SetFloat("Speed", _speed);
    //     _animator.SetFloat("State", _state);
    //     _animator.SetFloat("Vert", _vert);
    // }
}
