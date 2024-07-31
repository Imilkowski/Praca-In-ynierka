using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ShootProjectile();
    }

    private void ShootProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, Quaternion.identity, null).GetComponent<Projectile>().Shoot(transform.forward);
    }
}
