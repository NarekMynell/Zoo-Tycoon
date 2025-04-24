using UnityEngine;

public class InputDisabler : MonoBehaviour
{
    public static bool IsInputEnabled => _value == 0;
    private static int _value = 0;

    private void OnEnable()
    {
        _value++;  
    }

    private void OnDisable()
    {
        _value--;
    }
}