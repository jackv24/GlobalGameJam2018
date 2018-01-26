using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerProjectile : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float lifeTime = 5.0f;
    private float lifeEndTime;

    public GameObject explosionPrefab;

    public int damageValue = 1;

    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        lifeEndTime = Time.time + lifeTime;
    }

    private void OnDisable()
    {
        body.velocity = Vector2.zero;
    }

    private void Update()
    {
        if(Time.time >= lifeEndTime)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlatformerStats stats = collision.GetComponent<PlatformerStats>();

            if(stats)
            {
                stats.RemoveHealth(damageValue);
            }
        }

        Explode();
    }

    public void Fire(float direction)
    {
        Vector3 velocity = body.velocity;
        velocity.x = direction * moveSpeed;
        body.velocity = velocity;
    }

    public void Explode()
    {
        if (explosionPrefab)
        {
            GameObject obj = ObjectPooler.GetPooledObject(explosionPrefab);
            obj.transform.position = transform.position;
        }

        gameObject.SetActive(false);
    }
}
