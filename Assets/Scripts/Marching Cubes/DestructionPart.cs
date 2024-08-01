using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionPart : MonoBehaviour
{
    public Vector3Int partPos;

    void OnCollisionEnter(Collision collision)
    {
        transform.parent.GetComponent<DestructionModel>().ImpactReceived(collision, partPos);
    }
}
