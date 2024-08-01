using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionModel : MarchingCubesModel
{
    [Header("Properties")]

    public int collisionRadius;

    public void ImpactReceived(Collision collision, Vector3Int partPos)
    {
        Destruct(collision.contacts[0].point - transform.position + (collision.contacts[0].normal * voxelModel.resolution * 0.5f), partPos);
    }

    private void Destruct(Vector3 collisionPoint, Vector3Int partPos)
    {
        collisionPoint = collisionPoint / voxelModel.resolution;
        Vector3Int voxelCollisionPoint = new Vector3Int((int)Mathf.Round(collisionPoint.x), (int)Mathf.Round(collisionPoint.y), (int)Mathf.Round(collisionPoint.z));

        List<Vector3Int> voxelOffsets = VoxelsInSphere();
        foreach (Vector3Int voxelOffset in voxelOffsets)
        {
            try
            {
                voxelDataArray[voxelCollisionPoint.x + voxelOffset.x, voxelCollisionPoint.y + voxelOffset.y, voxelCollisionPoint.z + voxelOffset.z] = false;
            }
            catch (System.Exception)
            {
                continue;
            }
        }

        CalculateModel(GetIdPyPos(partPos));
    }

    private List<Vector3Int> VoxelsInSphere() //TODO: save this info in projectiles
    {
        List<Vector3Int> voxelPoints = new List<Vector3Int>();

        for (int x = -collisionRadius; x <= collisionRadius; x++)
        {
            for (int y = -collisionRadius; y <= collisionRadius; y++)
            {
                for (int z = -collisionRadius; z <= collisionRadius; z++)
                {
                    Vector3Int point = new Vector3Int(x, y, z);

                    if(Vector3.Distance(Vector3.zero, point) <= collisionRadius)
                    {
                        voxelPoints.Add(point);
                    }
                }
            }
        }

        return voxelPoints;
    }
}
