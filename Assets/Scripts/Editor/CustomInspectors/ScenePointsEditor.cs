using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ScenePoints))]
public class ScenePointsEditor : Editor
{
    private ScenePoints sp;
    private bool snapToMesh = true;
    private int generateCount = 10;
    private MeshCollider targetMesh;

    private void OnEnable()
    {
        sp = (ScenePoints)target;
        SceneView.duringSceneGui += OnScene;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnScene;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        GUILayout.Label("Point Generation", EditorStyles.boldLabel);

        targetMesh = (MeshCollider)EditorGUILayout.ObjectField("Target Mesh", targetMesh, typeof(MeshCollider), true);
        generateCount = EditorGUILayout.IntField("Points Count", generateCount);
        snapToMesh = EditorGUILayout.Toggle("Snap to Scene Mesh", snapToMesh);

        if (GUILayout.Button("Generate Points"))
        {
            Undo.RecordObject(sp, "Generate Scene Points");
            Vector3[] points = GenerateRandomPoints(targetMesh, generateCount, snapToMesh);
            sp.SetPoints(points);
            EditorUtility.SetDirty(sp);
        }
    }

    private void OnScene(SceneView sceneView)
    {
        if (sp == null || sp.Points == null) return;

        Vector3[] points = sp.Points;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 point = points[i];
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(point, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                if (snapToMesh)
                {
                    if (Physics.Raycast(newPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 100f))
                    {
                        newPos = hit.point;
                    }
                }

                Undo.RecordObject(sp, "Move Point");
                points[i] = newPos;
                sp.SetPoints(points);
                EditorUtility.SetDirty(sp);
            }

            Handles.SphereHandleCap(0, point, Quaternion.identity, 0.2f, EventType.Repaint);
        }
    }

    private Vector3[] GenerateRandomPoints(MeshCollider meshCol, int count, bool snap)
    {
        if (meshCol == null || meshCol.sharedMesh == null)
        {
            Debug.LogWarning("MeshCollider missing.");
            return new Vector3[count];
        }

        Mesh mesh = meshCol.sharedMesh;
        Vector3[] verts = mesh.vertices;
        int[] tris = mesh.triangles;
        Transform meshTr = meshCol.transform;

        List<Vector3> result = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            int triIndex = Random.Range(0, tris.Length / 3) * 3;

            Vector3 a = meshTr.TransformPoint(verts[tris[triIndex]]);
            Vector3 b = meshTr.TransformPoint(verts[tris[triIndex + 1]]);
            Vector3 c = meshTr.TransformPoint(verts[tris[triIndex + 2]]);
            Vector3 point = RandomPointInTriangle(a, b, c);

            if (snap)
            {
                if (Physics.Raycast(point + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 100f))
                {
                    point = hit.point;
                }
            }

            result.Add(point);
        }

        return result.ToArray();
    }

    private Vector3 RandomPointInTriangle(Vector3 a, Vector3 b, Vector3 c)
    {
        float r1 = Random.value;
        float r2 = Random.value;

        if (r1 + r2 >= 1f)
        {
            r1 = 1f - r1;
            r2 = 1f - r2;
        }

        return a + r1 * (b - a) + r2 * (c - a);
    }
}
