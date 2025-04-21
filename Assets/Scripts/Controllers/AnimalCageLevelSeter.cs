using UnityEngine;

public class AnimalCageLevelSeter : LevelSeter<AnimalCageLevelSetData>
{
    public override void SetLevel(int level)
    {
        if(level > _levels.Length)
        {
            Debug.LogError("Index out of range exception");
            return;
        }

        foreach (var obj in _levels[level-1].objectsToDisable)
        {
            obj.SetActive(false);
        }

        foreach (var obj in _levels[level-1].objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}


[System.Serializable]
    public class AnimalCageLevelSetData : LevelSetData
    {
        public GameObject[] objectsToEnable;
        public GameObject[] objectsToDisable;
    }
