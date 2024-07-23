using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MarchingCubesModel : MonoBehaviour
{
    public VoxelModel voxelModel;

    private MeshFilter _meshFilter;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private bool[,,] voxelDataArray;

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();

        ConvertVoxelDataToArray();

        MarchCubes();
        
        SetMesh();
    }

    private void SetMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        _meshFilter.mesh = mesh;
    }

    private void MarchCubes()
    {
        vertices.Clear();
        triangles.Clear();

        for (int x = 0; x < voxelModel.voxelModelSize.x; x++)
        {
            for (int y = 0; y < voxelModel.voxelModelSize.y; y++)
            {
                for (int z = 0; z < voxelModel.voxelModelSize.z; z++)
                {
                    bool[] cubeCorners = new bool[8];

                    for (int i = 0; i < 8; i++)
                    {
                        Vector3Int corner = new Vector3Int(x, y, z) + MarchingTable.Corners[i]; //TODO: probably multiply by voxelModel.resolution
                        cubeCorners[i] = voxelDataArray[corner.x, corner.y, corner.z];
                    }
                }
            }
        }
    }

    private void ConvertVoxelDataToArray()
    {
        voxelDataArray = new bool[voxelModel.voxelModelSize.x, voxelModel.voxelModelSize.y, voxelModel.voxelModelSize.z]; //TODO: is a new initialized array of bools filled with falses?

        foreach (Voxel voxel in voxelModel.voxelData)
        {
            voxelDataArray[(int)voxel.pos.x, (int)voxel.pos.y, (int)voxel.pos.z] = true;
        }
    }
}
