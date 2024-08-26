using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionPart : MonoBehaviour, IDamageable
{
    public Vector3Int partPos;
    public int partId;
    public Vector3Int[] partBounds;

    public void Damage(float damage, Vector3 collisionPoint)
    {
        transform.parent.GetComponent<DestructionModel>().ImpactReceived(collisionPoint, partId, (int)damage);
        Debug.Log("Shoot part: " + partId + ", collision point: " + collisionPoint);
    }

    void OnCollisionEnter(Collision collision)
    {
        transform.parent.GetComponent<DestructionModel>().ImpactReceived(collision.contacts[0].point, partId, 3);
    }
}
