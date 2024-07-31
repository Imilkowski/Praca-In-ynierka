using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Properties")]

    [SerializeField] private float _force;

    public void Shoot(Vector3 forwardVector)
    {
        GetComponent<Rigidbody>().AddForce(forwardVector * _force);

        Invoke("DestroyAfterTime", 5);
    }

    private void DestroyAfterTime()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
