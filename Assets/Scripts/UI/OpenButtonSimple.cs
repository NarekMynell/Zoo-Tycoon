using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class OpenButtonSimple : MonoBehaviour
{
    [SerializeField] private GameObject _objectToDisable;
    
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Open);
    }

    private void Open()
    {
        _objectToDisable.SetActive(true);
    }
}