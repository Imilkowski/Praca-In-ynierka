using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionPart : MonoBehaviour
{
    public Vector3Int partPos;
    public int partId;
    public Vector3Int[] partBounds;

    void OnCollisionEnter(Collision collision)
    {
        transform.parent.GetComponent<DestructionModel>().ImpactReceived(collision, partId);
    }
}
