using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerAttack : MonoBehaviour
{
    public GameObject projectilePrefab;

    public Transform muzzle;
    public float fireRate = 2.0f;
    private float nextFireTime;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void Shoot(float direction)
    {
        if(projectilePrefab && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + (1 / fireRate);

            GameObject projectileObj = ObjectPooler.GetPooledObject(projectilePrefab);

            projectileObj.transform.position = muzzle.position;

            Vector3 scale = projectileObj.transform.localScale;
            scale.x = projectilePrefab.transform.localScale.x * direction;
            projectileObj.transform.localScale = scale;

            PlatformerProjectile proj = projectileObj.GetComponent<PlatformerProjectile>();
            if (proj)
                proj.Fire(direction);

            Collider2D projectileCollider = projectileObj.GetComponent<Collider2D>();
            if (projectileCollider)
                Physics2D.IgnoreCollision(col, projectileCollider);
        }
    }
}
