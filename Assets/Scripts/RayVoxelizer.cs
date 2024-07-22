using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Voxel
{
    public Vector3 pos;

    public Voxel(Vector3 pos)
    {
        this.pos = pos;
    }
}

public class RayVoxelizer : MonoBehaviour
{
    public MeshCollider meshCollider;
    [Range(0.025f, 1f)] public float resolution;

    public VoxelModel destinationVoxelData;

    [Header("Debug")]

    public bool showBoundingBox;
    public bool showVoxelGrid;

    private List<Voxel> voxelData;

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
            boundingBoxSize /= resolution;

            int voxels = (int)Mathf.Ceil(boundingBoxSize.x * boundingBoxSize.y * boundingBoxSize.z);

            if (voxels > 10000)
                return;

            for (int z = 0; z < boundingBoxSize.z; z++)
            {
                for (int y = 0; y < boundingBoxSize.y; y++)
                {
                    for (int x = 0; x < boundingBoxSize.x; x++)
                    {
                        Gizmos.DrawSphere(boundingBoxPivot + (new Vector3(x, y, z) * resolution) + (Vector3.one * resolution / 2), resolution / 10);
                    }
                }
            }
        }
    }

    public void GenerateVoxelData()
    {
        StartCoroutine(GenerateData());
    }

    public IEnumerator GenerateData()
    {
        voxelData = new List<Voxel>();

        Bounds bounds = meshCollider.bounds;

        Vector3 boundingBoxSize = bounds.extents * 2;
        Vector3 boundingBoxPivot = bounds.center - (boundingBoxSize / 2);

        boundingBoxSize /= resolution;

        for (int z = 0; z < boundingBoxSize.z; z++)
        {
            for (int y = 0; y < boundingBoxSize.y; y++)
            {
                for (int x = 0; x < boundingBoxSize.x; x++)
                {
                    Vector3 pointPos = boundingBoxPivot + (new Vector3(x, y, z) * resolution) + (Vector3.one * resolution / 2);
                    if (CheckPoint(pointPos))
                        voxelData.Add(new Voxel(pointPos));
                }
            }

            Debug.Log("Generating in progress " + Mathf.Round(((z + 1) / boundingBoxSize.z) * 100) + "%");
            yield return new WaitForEndOfFrame();
        }

        destinationVoxelData.voxelData = voxelData;
        //destinationVoxelData.voxelModelSize = voxelDataSize;
        destinationVoxelData.resolution = resolution;

        Debug.Log("Voxel Data Generated: " + voxelData.Count + " voxels");
    }

    private bool CheckPoint(Vector3 pointPos)
    {
        List<RaycastHit> raycastHits = new List<RaycastHit>();

        RaycastHit rightHit;
        bool right = Physics.Raycast(pointPos + Vector3.right * 1000, Vector3.left, out rightHit, 1000);
        if (right)
            raycastHits.Add(rightHit);

        RaycastHit leftHit;
        bool left = Physics.Raycast(pointPos + Vector3.left * 1000, Vector3.right, out leftHit, 1000);
        if (left)
            raycastHits.Add(leftHit);

        RaycastHit forwardHit;
        bool forward = Physics.Raycast(pointPos + Vector3.forward * 1000, Vector3.back, out forwardHit, 1000);
        if (forward)
            raycastHits.Add(forwardHit);

        RaycastHit backHit;
        bool back = Physics.Raycast(pointPos + Vector3.back * 1000, Vector3.forward, out backHit, 1000);
        if (back)
            raycastHits.Add(backHit);

        RaycastHit upHit;
        bool up = Physics.Raycast(pointPos + Vector3.up * 1000, Vector3.down, out upHit, 1000);
        if (up)
            raycastHits.Add(upHit);

        RaycastHit downHit;
        bool down = Physics.Raycast(pointPos + Vector3.down * 1000, Vector3.up, out downHit, 1000);
        if (down)
            raycastHits.Add(downHit);

        if (raycastHits.Count == 6)
            return true;
        else
            return false;
    }
}
