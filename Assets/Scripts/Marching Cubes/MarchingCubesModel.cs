using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MarchingCubesModel : MonoBehaviour
{
    [Header("Properties")]

    public VoxelModel voxelModel;
    public MeshCollider meshCollider;

    public bool showVoxelPoints;

    [HideInInspector] public MeshFilter meshFilter;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    [HideInInspector] public bool[,,] voxelDataArray;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        ConvertVoxelDataToArray();

        CalculateModel();
    }

    public void CalculateModel()
    {
        MarchCubes();
        SetMesh();
    }

    private void SetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    private void MarchCubes()
    {
        vertices.Clear();
        triangles.Clear();

        for (int x = 0; x < voxelModel.voxelModelSize.x - 1; x++)
        {
            for (int y = 0; y < voxelModel.voxelModelSize.y - 1; y++)
            {
                for (int z = 0; z < voxelModel.voxelModelSize.z - 1; z++)
                {
                    bool[] cubeCorners = new bool[8];

                    for (int i = 0; i < 8; i++)
                    {
                        Vector3Int corner = new Vector3Int(x, y, z) + MarchingTable.Corners[i];
                        cubeCorners[i] = !voxelDataArray[corner.x, corner.y, corner.z]; //why does it need to be inverted?
                    }

                    MarchCube(new Vector3Int(x, y, z), GetConfigurationIndex(cubeCorners));
                }
            }
        }
    }

    private void MarchCube(Vector3Int position, int configIndex)
    {
        if (configIndex == 0 || configIndex == 255)
            return;

        int edgeIndex = 0;
        for (int t = 0; t < 5; t++)
        {
            for (int v = 0; v < 3; v++)
            {
                int triTableValue = MarchingTable.Triangles[configIndex, edgeIndex];

                if (triTableValue == -1)
                    return;

                Vector3 edgeStart = position + MarchingTable.Edges[triTableValue, 0];
                Vector3 edgeEnd = position + MarchingTable.Edges[triTableValue, 1];

                Vector3 vertex = (edgeStart + edgeEnd) / 2 * voxelModel.resolution;

                vertices.Add(vertex);
                triangles.Add(vertices.Count - 1);

                edgeIndex++;
            }
        }
    }

    private int GetConfigurationIndex(bool[] cubeCorners)
    {
        int configId = 0;

        for (int i = 0; i < 8; i++)
        {
            if(cubeCorners[i])
                configId |= 1 << i;
        }

        return configId;
    }

    private void ConvertVoxelDataToArray()
    {
        voxelDataArray = new bool[voxelModel.voxelModelSize.x, voxelModel.voxelModelSize.y, voxelModel.voxelModelSize.z];

        foreach (Voxel voxel in voxelModel.voxelData)
        {
            voxelDataArray[(int)voxel.pos.x, (int)voxel.pos.y, (int)voxel.pos.z] = true;
        }
    }

    void OnDrawGizmos()
    {
        if (!showVoxelPoints)
            return;

        if (voxelDataArray == null)
            return; 

        for (int x = 0; x < voxelModel.voxelModelSize.x; x++)
        {
            for (int y = 0; y < voxelModel.voxelModelSize.y; y++)
            {
                for (int z = 0; z < voxelModel.voxelModelSize.z; z++)
                {
                    if(voxelDataArray[x, y, z])
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(transform.position + (new Vector3(x, y, z) * voxelModel.resolution), 0.01f);
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(transform.position + (new Vector3(x, y, z) * voxelModel.resolution), 0.01f);
                    }
                }
            }
        }
    }
}
