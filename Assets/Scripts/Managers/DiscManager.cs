using UnityEngine;

public class DiscManager : MonoBehaviour
{
    private string _filePath = "/saveData.json";

    private void Awake()
    {
        LoadData();
        DontDestroyOnLoad(gameObject);
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
        GameData.serializableData ??= new SerializableData(GameData.businesses.Length);

        GameData.serializableData.exitDateTime = System.DateTime.Now.ToString("o");
        GameData.serializableData.exitTotalMoney = GameData.totalMoney;
        GameData.serializableData.sound = GameData.sound;
        GameData.serializableData.music = GameData.music;

        for (int i = 0; i < GameData.businesses.Length; i++)
        {
            BusinessBehaviour business = GameData.businesses[i];
            
            GameData.serializableData.businesses[i].accumulatedMoney = business.AccumulatedMoney;
            GameData.serializableData.businesses[i].level = business.CurrentLevel;
        }

        string jsonData = JsonUtility.ToJson(GameData.serializableData);
        System.IO.File.WriteAllText(Application.persistentDataPath + _filePath, jsonData);
    }

    private void LoadData()
    {
        string filePath = Application.persistentDataPath + _filePath;
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            GameData.serializableData = JsonUtility.FromJson<SerializableData>(jsonData);
        }
    }
}