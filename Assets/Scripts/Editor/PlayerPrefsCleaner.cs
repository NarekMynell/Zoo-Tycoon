using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class PlayerPrefsCleaner : Editor
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        ClearDirectoryContents(Application.persistentDataPath);
        Debug.Log("PlayerPrefs data has been cleared.");
    }

    private static void ClearDirectoryContents(string directoryPath)
    {
        // Check if the directory exists
        if (Directory.Exists(directoryPath))
        {
            // Clear files in the directory
            Array.ForEach(Directory.GetFiles(directoryPath), File.Delete);

            // Clear subdirectories and their contents
            Array.ForEach(Directory.GetDirectories(directoryPath), dir => ClearDirectoryContents(dir));
        }
    }
}
