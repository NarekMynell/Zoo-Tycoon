using UnityEngine;

public class LevelVisualSeter : MonoBehaviour
{
    [SerializeField] protected LevelVisualData[] _levels;


    public void Init(int level)
    {
        SetLevel(level);

        for(int i = level; i < _levels.Length; i++)
        {
            foreach (var obj in _levels[i].objectsToEnable)
            {
                obj.SetActive(false);
            }

            foreach (var obj in _levels[i].objectsToDisable)
            {
                obj.SetActive(true);
            }
        }
    }

    public void SetLevel(int level)
    {
        if(level > _levels.Length)
        {
            Debug.LogError("Index out of range exception");
            return;
        }

        if(level > 0)
        {
            for(int i = 0; i < level; i++)
            {
                foreach (var obj in _levels[i].objectsToDisable)
                {
                    obj.SetActive(false);
                }
                
                foreach (var obj in _levels[i].objectsToEnable)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}

[System.Serializable]
public class LevelVisualData
{
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;
}