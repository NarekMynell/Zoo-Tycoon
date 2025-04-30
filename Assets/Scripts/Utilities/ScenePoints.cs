using UnityEngine;

public class ScenePoints : MonoBehaviour
{
    [SerializeField] private Vector3[] _points;
    public Vector3[] Points => _points;

#if UNITY_EDITOR
    public void SetPoints(Vector3[] newPoints)
    {
        _points = newPoints;
    }
#endif
}
