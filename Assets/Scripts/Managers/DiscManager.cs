using UnityEngine;

public class DiscManager : MonoBehaviour
{
    private void Awake()
    {
        LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }

    private void SaveData()
    {

    }

    private void LoadData()
    {

    }
}