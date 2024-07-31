using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionModel : MarchingCubesModel
{
    void OnCollisionEnter(Collision collision)
    {
        Destruct(collision.contacts[0].point - transform.position + (collision.contacts[0].normal * voxelModel.resolution * 0.5f));
    }

    private void Destruct(Vector3 collisionPoint)
    {
        collisionPoint = collisionPoint / voxelModel.resolution;
        Vector3Int voxelCollisionPoint = new Vector3Int((int)Mathf.Round(collisionPoint.x), (int)Mathf.Round(collisionPoint.y), (int)Mathf.Round(collisionPoint.z));

        voxelDataArray[voxelCollisionPoint.x, voxelCollisionPoint.y, voxelCollisionPoint.z] = false;
        voxelDataArray[voxelCollisionPoint.x + 1, voxelCollisionPoint.y, voxelCollisionPoint.z] = false;
        voxelDataArray[voxelCollisionPoint.x - 1, voxelCollisionPoint.y, voxelCollisionPoint.z] = false;
        voxelDataArray[voxelCollisionPoint.x, voxelCollisionPoint.y + 1, voxelCollisionPoint.z] = false;
        voxelDataArray[voxelCollisionPoint.x, voxelCollisionPoint.y - 1, voxelCollisionPoint.z] = false;
        voxelDataArray[voxelCollisionPoint.x, voxelCollisionPoint.y, voxelCollisionPoint.z + 1] = false;
        voxelDataArray[voxelCollisionPoint.x, voxelCollisionPoint.y, voxelCollisionPoint.z - 1] = false;

        CalculateModel();
    }
}
