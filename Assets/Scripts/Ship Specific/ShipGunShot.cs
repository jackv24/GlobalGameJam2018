using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipGunShot : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;

    [SerializeField]
    [Range(1, 5)]
    [Tooltip("Time in seconds the shot will stay live")]
    private float lifetime = 3;

    [SerializeField]
    private float velocity = 10;
    
    public Vector2 Position { set { transform.position = value; } }
    public Vector2 Direction { get; set; }

    private void Awake()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        this.rigidbody2D.gravityScale = 0;
    }

    private void OnEnable()
    {
        StartCoroutine(Lifetime());
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = Direction.normalized * velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: Apply damage to hit targets that implement IShipDamageable
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);

        this.gameObject.SetActive(false);
    }
}
