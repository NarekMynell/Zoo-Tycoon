using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameobjectPooler : MonoBehaviour, IPooler<GameObject>, IEnumerable<GameObject>
{
    private Queue<GameObject> _queue = new();
    private HashSet<GameObject> _activeInstances = new();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            _queue.Enqueue(child.gameObject);
        }
    }

    public GameObject ForceGetInstance()
    {
        if (_queue.Count > 0)
        {
            var instance = _queue.Dequeue();
            _activeInstances.Add(instance);
            return instance;
        }
        else
        {
            var instance = _activeInstances.FirstOrDefault();
            return instance;
        }
    }

    public bool TryGetInstance(out GameObject instance)
    {
        if (_queue.Count > 0)
        {
            instance = _queue.Dequeue();
            _activeInstances.Add(instance);
            return true;
        }
        else
        {
            instance = null;
            return false;
        }
    }

    public void ReturnInstance(GameObject instance)
    {
        if (_activeInstances.Contains(instance))
        {
            _activeInstances.Remove(instance);
            _queue.Enqueue(instance);
        }
        else
        {
            Debug.LogError("Instance not found in active instances.");
        }
    }

    public IEnumerator<GameObject> GetEnumerator()
    {
        foreach (var instance in _activeInstances)
        {
            yield return instance;
        }

        foreach (var instance in _queue)
        {
            yield return instance;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
