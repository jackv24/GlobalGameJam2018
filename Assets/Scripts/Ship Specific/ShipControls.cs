using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipControls : Controllable
{

    #region Delegates & Events
    public delegate void TransmissionPingDelegate(ShipControls sender);

    public event TransmissionPingDelegate OnTransmissionPing;
    #endregion

    #region Fields & Properties
    private new Rigidbody2D rigidbody2D;
    private ResourceBank resourceBank;
    private ShipResourceGlowEffect glowEffect;

    private new Collider2D collider;

    private ShipGun powerWeapon;
    private float lastPingTime;
    private Collider2D[] pingResourceCache = new Collider2D[64];
    private Dictionary<ShipControls, Vector2> enemyPings;

    private float lookAngle;

    [SerializeField]
    private float maximumVelocity = 10;

    [SerializeField]
    [Range(10, 30)]
    private float impulseMagnitude = 15;

    [SerializeField]
    [Range(1, 30)]
    [Tooltip("Time in seconds between Transmission Pings")]
    private float transmissionPingCooldown = 5;

    [SerializeField]
    [Range(10, 100)]
    private float transmissionPingResourceRadius = 30;

    [SerializeField]
    private ShipGun defaultWeapon;
    #endregion

    private void Awake()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        this.rigidbody2D.gravityScale = 0;

        this.resourceBank = GetComponentInParent<ResourceBank>();
        if (this.resourceBank == null)
            throw new System.Exception("ShipControls script requires a ResourceBank script to be present somewhere in the parent hierarchy");

        this.defaultWeapon = GetComponent<ShipGun>();
        if (this.defaultWeapon == null)
            throw new System.MissingFieldException("ShipControls script requires a default weapon");

        this.collider = GetComponent<Collider2D>();
        this.enemyPings = new Dictionary<ShipControls, Vector2>();
    }

    private void Start()
    {
        // Commence hacky shit to get glow post processor
        PlayerInput inputScript = GetComponentInParent<PlayerInput>();
        glowEffect = inputScript.cameraRender.GetComponent<ShipResourceGlowEffect>();
    }

    #region Collision Detection
    /// <summary>
    /// Collects the resources from an object in the world
    /// </summary>
    private void CollectResource(Transform worldObject)
    {
        Resource resource = worldObject.gameObject.GetComponent<Resource>();

        if (resource == null)
            throw new System.Exception(string.Format("No Resource script found on resource-tagged object with name {0}", worldObject.name));

        resourceBank.AddResources(resource);
    }

    /// <summary>
    /// Equips a gun found on an object
    /// </summary>
    private void CollectWeapon(Transform worldObject)
    {
        ShipGunPickup gunPickup = worldObject.GetComponent<ShipGunPickup>();

        if (gunPickup == null)
            throw new System.Exception(string.Format("No ShipGunPickup script found on weapon-tagged object with name {0}", worldObject.name));

        ShipGunData gunData = gunPickup.Consume();
        powerWeapon = this.gameObject.AddComponent<ShipGun>();
        powerWeapon.ShipGunData = gunData;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.transform.tag)
        {
            case "Resource":

                CollectResource(collider.transform);
                break;

            case "ShipGun":

                CollectWeapon(collider.transform);
                break;

            default: break;
        }
    }
    #endregion

    #region Controllable Overrides
    private Vector2 movementVector;

    public override void Move(Vector2 inputVector)
    {
        movementVector = inputVector;
    }

    private Vector2 GetLookVector()
    {
        return new Vector2(Mathf.Sin(Mathf.Deg2Rad * lookAngle), Mathf.Cos(Mathf.Deg2Rad * lookAngle));
    }

    public override void Look(Vector2 inputVector)
    {
        lookAngle = Mathf.LerpAngle(lookAngle, Vector2.SignedAngle(Vector2.up, inputVector), inputVector.sqrMagnitude * 0.1f);
        
        rigidbody2D.MoveRotation(lookAngle);
    }

    /// <summary>
    /// Ships use the Jump function to ping transmissions
    /// </summary>
    public override void Jump(ButtonState buttonState)
    {
        if (buttonState == ButtonState.WasPressed
            && Time.time - lastPingTime >= transmissionPingCooldown)
        {
            int resourcesInRange = Physics2D.OverlapCircleNonAlloc(transform.position, transmissionPingResourceRadius, pingResourceCache);

            glowEffect.Set(resourcesInRange, pingResourceCache.Select(resource => (Vector2)resource.transform.position).ToArray());


            // Raise ping event
            if (OnTransmissionPing != null)
                OnTransmissionPing(this);

            lastPingTime = Time.time;
        }
    }

    public override void Shoot(ButtonState buttonState)
    {
        if (buttonState == ButtonState.WasPressed)
        {
            ShipGunShot shot;

            if (powerWeapon != null)
            {
                // Shoot power weapon & drop it
                shot = powerWeapon.Shoot(transform.position, transform.up);
                powerWeapon = null;
            }
            else
            {
                shot = defaultWeapon.Shoot(transform.position, transform.up);
            }
            
            Physics2D.IgnoreCollision(shot.Collider, collider);
        }
    }
    #endregion

    /// <summary>
    /// Alerts this ship to the position of an enemy
    /// </summary>
    public void AlertEnemyPresence(ShipControls enemyShip)
    {
        if (enemyPings.ContainsKey(enemyShip))
            enemyPings[enemyShip] = enemyShip.transform.position;
        else
            enemyPings.Add(enemyShip, enemyShip.transform.position);
    }

    private void FixedUpdate()
    {
        Vector2 desiredVelocity = rigidbody2D.velocity + movementVector * impulseMagnitude * Time.fixedDeltaTime;

        if (desiredVelocity.sqrMagnitude > maximumVelocity * maximumVelocity)
            desiredVelocity = desiredVelocity.normalized * maximumVelocity;

        rigidbody2D.velocity = desiredVelocity;
    }
}
