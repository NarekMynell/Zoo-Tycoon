using UnityEngine;

public abstract class LevelSeter<T> : MonoBehaviour where T : LevelSetData
{
    [SerializeField] protected T[] _levels;

    public abstract void SetLevel(int level);
}

    [System.Serializable]
    public class LevelSetData
    {

    }