using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerAttack : MonoBehaviour
{
    public GameObject projectilePrefab;

    public Transform muzzle;
    public float fireRate = 2.0f;
    private float nextFireTime;

    public GameObject muzzleFlashPrefab;

    public AudioEvent fireSound;

    private float shootDir;
    private bool shouldShoot = false;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void LateUpdate()
    {
        if(shouldShoot)
        {
            shouldShoot = false;

            if (projectilePrefab && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + (1 / fireRate);

                GameObject muzzleFlash = ObjectPooler.GetPooledObject(muzzleFlashPrefab);
                muzzleFlash.transform.SetParent(null);
                muzzle.transform.localScale = new Vector3(Mathf.Sign(-shootDir), 1, 1);
                muzzleFlash.transform.SetParent(muzzle);
                muzzleFlash.transform.localPosition = Vector2.zero;

                GameObject projectileObj = ObjectPooler.GetPooledObject(projectilePrefab);

                projectileObj.transform.position = muzzle.position;

                Vector3 scale = projectileObj.transform.localScale;
                scale.x = projectilePrefab.transform.localScale.x * shootDir;
                projectileObj.transform.localScale = scale;

                PlatformerProjectile proj = projectileObj.GetComponent<PlatformerProjectile>();
                if (proj)
                    proj.Fire(shootDir);

                fireSound.Play(transform.position);

                Collider2D projectileCollider = projectileObj.GetComponent<Collider2D>();
                if (projectileCollider)
                    Physics2D.IgnoreCollision(col, projectileCollider);
            }
        }
    }

    public void Shoot(float direction)
    {
        shouldShoot = true;
        shootDir = direction;
    }
}
