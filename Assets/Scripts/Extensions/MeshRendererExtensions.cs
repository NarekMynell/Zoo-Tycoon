using UnityEngine;

public static class MeshFilterExtensions
{
    /// <summary>
    /// Gets a random point on the current surface of the specified mesh.
    /// </summary>
    /// <param name="meshFilter">The MeshFilter component containing the mesh to sample from.</param>
    /// <param name="usePreciseCalculation">If true, uses a precise calculation that accounts for triangle areas, which can be computationally expensive, especially for dense meshes. If false, selects a random triangle without area weighting for faster performance.</param>
    /// <returns>A Vector3 representing a random point on the mesh surface in world coordinates.</returns>
    /// <remarks>
    /// When <paramref name="usePreciseCalculation"/> is true, the operation may be significantly slower for meshes with a large number of triangles, particularly dense meshes.
    /// </remarks>
    public static Vector3 GetRandomPointOnSurface(this MeshFilter meshFilter, bool usePreciseCalculation = false)
    {
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("MeshFilter is null");
            return Vector3.zero;
        }

        Mesh mesh = meshFilter.mesh;
        int selectedTriangle = 0;

        if(usePreciseCalculation)
        {
            float[] areas = new float[mesh.triangles.Length / 3];
            float totalArea = 0f;

            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 v0 = mesh.vertices[mesh.triangles[i]];
                Vector3 v1 = mesh.vertices[mesh.triangles[i + 1]];
                Vector3 v2 = mesh.vertices[mesh.triangles[i + 2]];

                float area = Vector3.Cross(v1 - v0, v2 - v0).magnitude * 0.5f;
                areas[i / 3] = area;
                totalArea += area;
            }

            float randomArea = Random.value * totalArea;
            float cumulativeArea = 0f;

            for (int i = 0; i < areas.Length; i++)
            {
                cumulativeArea += areas[i];
                if (randomArea <= cumulativeArea)
                {
                    selectedTriangle = i * 3;
                    break;
                }
            }
        }
        else
        {
            // 1. Pick a random triangle
            selectedTriangle = Random.Range(0, mesh.triangles.Length / 3) * 3;
        }



        Vector3 a = mesh.vertices[mesh.triangles[selectedTriangle]];
        Vector3 b = mesh.vertices[mesh.triangles[selectedTriangle + 1]];
        Vector3 c = mesh.vertices[mesh.triangles[selectedTriangle + 2]];

        float r1 = Random.value;
        float r2 = Random.value;
        if (r1 + r2 > 1)
        {
            r1 = 1 - r1;
            r2 = 1 - r2;
        }

        Vector3 point = a + r1 * (b - a) + r2 * (c - a);

        return meshFilter.transform.TransformPoint(point);
    }
}