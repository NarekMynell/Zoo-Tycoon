using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class CloseButtonSimple : MonoBehaviour
{
    [SerializeField] private GameObject _objectToDisable;
    
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Close);
    }

    private void Close()
    {
        _objectToDisable.SetActive(false);
    }
}