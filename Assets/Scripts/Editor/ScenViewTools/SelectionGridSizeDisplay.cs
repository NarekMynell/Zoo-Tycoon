using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class SelectionGridSizeDisplay
{
    static SelectionGridSizeDisplay()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (Selection.gameObjects.Length == 0)
            return;

        Bounds totalBounds = new Bounds();
        bool boundsInitialized = false;

        foreach (var go in Selection.gameObjects)
        {
            var renderers = go.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                if (!boundsInitialized)
                {
                    totalBounds = renderer.bounds;
                    boundsInitialized = true;
                }
                else
                {
                    totalBounds.Encapsulate(renderer.bounds);
                }
            }
        }

        if (!boundsInitialized)
            return;

        Vector3 size = totalBounds.size;

        GUIStyle titleStyle = new(GUI.skin.window)
        {
            alignment = TextAnchor.UpperLeft,
        };

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            wordWrap = true
        };

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(40, 5, 90, 80), "Grid Size", titleStyle);
        GUILayout.Label($"W:  {FormatSmart(size.x)}", labelStyle);
        GUILayout.Label($"H: {FormatSmart(size.y)}", labelStyle);
        GUILayout.Label($"D:  {FormatSmart(size.z)}", labelStyle);
        GUILayout.EndArea();
        Handles.EndGUI();
    }

    private static string FormatSmart(float value)
    {
        return Mathf.Approximately(value % 1f, 0f) ? value.ToString("F0") : value.ToString("F2");
    }
}