using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionModel : MarchingCubesModel
{
    [Header("Properties")]

    public int collisionRadius;

    public void ImpactReceived(Collision collision, int partId)
    {
        Destruct(collision.contacts[0].point - transform.position + (collision.contacts[0].normal * voxelModel.resolution * 0.5f), partId);
    }

    private void Destruct(Vector3 collisionPoint, int destructionPartId)
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

        //CalculateModel(GetIdPyPos(partPos));

        Vector3Int[] modificationBounds = new Vector3Int[2];
        modificationBounds[0] = voxelCollisionPoint - (Vector3Int.one * collisionRadius);
        modificationBounds[1] = voxelCollisionPoint + (Vector3Int.one * collisionRadius);

        //foreach (int partId in GetPartIdsByBoundingBox(modificationBounds))
        //{
        //    CalculateModel(partId);
        //}

        StartCoroutine(CalculateModelParts(GetPartIdsByBoundingBox(modificationBounds)));
    }

    private IEnumerator CalculateModelParts(List<int> partIds)
    {
        foreach (int partId in partIds)
        {
            CalculateModel(partId);

            yield return new WaitForEndOfFrame();
        }
    }

    private List<int> GetPartIdsByBoundingBox(Vector3Int[] modificationBounds)
    {
        List<int> partIds = new List<int>();

        foreach (GameObject modelPart in modelParts)
        {
            DestructionPart destructionPart = modelPart.GetComponent<DestructionPart>();
            Vector3Int[] partBounds = destructionPart.partBounds;

            if (CheckBoundsOverlap(modificationBounds, partBounds))
                partIds.Add(destructionPart.partId);
        }

        return partIds;
    }

    private bool CheckBoundsOverlap(Vector3Int[] modificationBounds, Vector3Int[] partBounds)
    {
        Vector3Int minA = modificationBounds[0];
        Vector3Int maxA = modificationBounds[1];

        Vector3Int minB = partBounds[0];
        Vector3Int maxB = partBounds[1];

        bool overlapX = minA.x <= maxB.x && maxA.x >= minB.x;
        bool overlapY = minA.y <= maxB.y && maxA.y >= minB.y;
        bool overlapZ = minA.z <= maxB.z && maxA.z >= minB.z;

        return overlapX && overlapY && overlapZ;
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
