using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Voxelizer : MonoBehaviour
{
    public MeshCollider meshCollider;
    [Range(0.1f, 1f)] public float resolution;

    public VoxelModel destinationVoxelData;

    [Header("Debug")]

    public bool showBoundingBox;
    public bool showVoxelGrid;

    private bool[,,] voxelData;

    void OnDrawGizmos()
    {
        Bounds bounds = meshCollider.bounds;

        Vector3 boundingBoxSize = bounds.extents * 2;
        Vector3 boundingBoxPivot = bounds.center - (boundingBoxSize / 2);

        if (showBoundingBox)
        {
            Handles.color = Color.red;

            Handles.DrawWireCube(bounds.center, boundingBoxSize);
        }

        if (showVoxelGrid)
        {
            Handles.color = Color.green;

            boundingBoxSize /= resolution;

            for (int z = 0; z < boundingBoxSize.z; z++)
            {
                for (int y = 0; y < boundingBoxSize.y; y++)
                {
                    for (int x = 0; x < boundingBoxSize.x; x++)
                    {
                        Handles.DrawWireCube(boundingBoxPivot + (new Vector3(x, y, z) * resolution) + (Vector3.one * resolution / 2), Vector3.one * resolution);
                    }
                }
            }
        }
    }

    public void GenerateVoxelData()
    {
        Bounds bounds = meshCollider.bounds;

        Vector3 boundingBoxSize = bounds.extents * 2;
        Vector3 boundingBoxPivot = bounds.center - (boundingBoxSize / 2);

        Vector3Int voxelDataSize = new Vector3Int((int)(boundingBoxSize.x / resolution), (int)(boundingBoxSize.y / resolution), (int)(boundingBoxSize.z / resolution));

        voxelData = new bool[voxelDataSize.x, voxelDataSize.y, voxelDataSize.z];

        for (int z = 0; z < voxelDataSize.z; z++)
        {
            for (int y = 0; y < voxelDataSize.y; y++)
            {
                for (int x = 0; x < voxelDataSize.x; x++)
                {
                    bool meshHit = Physics.BoxCast(boundingBoxPivot + new Vector3(x, y, z) + (Vector3.one * resolution / 2), Vector3.one * resolution / 2, Vector3.forward);
                    voxelData[x, y, z] = meshHit;
                }
            }
        }

        destinationVoxelData.voxelData = (bool[,,])voxelData.Clone();
        destinationVoxelData.voxelDataSize = voxelDataSize;

        //for (int j = 0; j < 10; j++)
        //{
        //    Debug.Log(destinationVoxelData.voxelData[Random.Range(0, destinationVoxelData.voxelDataSize.x), Random.Range(0, destinationVoxelData.voxelDataSize.y), Random.Range(0, destinationVoxelData.voxelDataSize.z)]);
        //}

        Debug.Log("Voxel Data Generated");
    }
}
