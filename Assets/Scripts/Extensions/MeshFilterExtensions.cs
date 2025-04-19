using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MeshRendererExtensions
{
    public static Vector3 GetRandomPointOnSurface(this MeshRenderer meshRenderer)
    {
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer-ը null է!");
            return Vector3.zero;
        }

        MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            return Vector3.zero;
        }

        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // Հաշվարկել եռանկյունների մակերեսները
        float[] areas = new float[triangles.Length / 3];
        float totalArea = 0f;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = vertices[triangles[i]];
            Vector3 v1 = vertices[triangles[i + 1]];
            Vector3 v2 = vertices[triangles[i + 2]];

            float area = Vector3.Cross(v1 - v0, v2 - v0).magnitude * 0.5f;
            areas[i / 3] = area;
            totalArea += area;
        }

        // Ընտրել պատահական եռանկյուն
        float randomArea = Random.value * totalArea;
        int selectedTriangle = 0;
        float cumulativeArea = 0f;

        for (int i = 0; i < areas.Length; i++)
        {
            cumulativeArea += areas[i];
            if (randomArea <= cumulativeArea)
            {
                selectedTriangle = i;
                break;
            }
        }

        // Ստանալ եռանկյունի գագաթները
        Vector3 a = vertices[triangles[selectedTriangle * 3]];
        Vector3 b = vertices[triangles[selectedTriangle * 3 + 1]];
        Vector3 c = vertices[triangles[selectedTriangle * 3 + 2]];

        // Գեներացնել բարիցենտրիկ կոորդինատներ
        float r1 = Random.value;
        float r2 = Random.value;
        if (r1 + r2 > 1)
        {
            r1 = 1 - r1;
            r2 = 1 - r2;
        }

        // Հաշվարկել կետը եռանկյունու վրա
        Vector3 point = a + r1 * (b - a) + r2 * (c - a);

        // Փոխակերպել world կոորդինատների՝ հաշվի առնելով position, rotation, scale
        return meshFilter.transform.TransformPoint(point);
    }
}
