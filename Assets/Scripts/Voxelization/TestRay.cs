using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRay : MonoBehaviour
{
    private List<RaycastHit> raycastHits;

    public int distance;

    void Update()
    {
        raycastHits = new List<RaycastHit>();

        RaycastHit rightHit;
        bool right = Physics.Raycast(transform.position + Vector3.right * distance, Vector3.left, out rightHit, distance);
        if(right)
            raycastHits.Add(rightHit);

        RaycastHit leftHit;
        bool left = Physics.Raycast(transform.position + Vector3.left * distance, Vector3.right, out leftHit, distance);
        if (left)
            raycastHits.Add(leftHit);

        RaycastHit forwardHit;
        bool forward = Physics.Raycast(transform.position + Vector3.forward * distance, Vector3.back, out forwardHit, distance);
        if (forward)
            raycastHits.Add(forwardHit);

        RaycastHit backHit;
        bool back = Physics.Raycast(transform.position + Vector3.back * distance, Vector3.forward, out backHit, distance);
        if (back)
            raycastHits.Add(backHit);

        RaycastHit upHit;
        bool up = Physics.Raycast(transform.position + Vector3.up * distance, Vector3.down, out upHit, distance);
        if (up)
            raycastHits.Add(upHit);

        RaycastHit downHit;
        bool down = Physics.Raycast(transform.position + Vector3.down * distance, Vector3.up, out downHit, distance);
        if (down)
            raycastHits.Add(downHit);

        Debug.Log(raycastHits.Count);
    }

    void OnDrawGizmos()
    {
        if (raycastHits == null)
            return;

        Debug.DrawRay(transform.position, Vector3.right * distance, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.left * distance, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.forward * distance, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.back * distance, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.up * distance, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.down * distance, Color.yellow);

        Gizmos.color = Color.red;

        foreach (RaycastHit hit in raycastHits)
        {
            Gizmos.DrawSphere(hit.point, 0.05f);
        }
    }
}
