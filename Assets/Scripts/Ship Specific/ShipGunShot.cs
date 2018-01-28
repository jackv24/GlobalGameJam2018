using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipGunShot : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;

    private new Collider2D collider;
    public Collider2D Collider { get { return collider; } }

    [SerializeField]
    [Range(1, 5)]
    [Tooltip("Time in seconds the shot will stay live")]
    private float lifetime = 3;

    [SerializeField]
    private float velocity = 10;
    
    public Vector2 Position { set { transform.position = value; } }
    public Vector2 Direction { get; set; }
    public Vector2 ExtraVelocity { get; set; }
    public int Damage { get; set; }

    private void Awake()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        this.rigidbody2D.gravityScale = 0;

        this.collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(Lifetime());
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = Direction.normalized * velocity + ExtraVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IShipDamageable damageable = collider.gameObject.GetComponent<IShipDamageable>();

        if (damageable != null)
            damageable.ApplyDamage(Damage);

        this.gameObject.SetActive(false);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);

        this.gameObject.SetActive(false);
    }
}
