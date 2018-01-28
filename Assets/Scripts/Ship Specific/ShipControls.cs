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

    private new Collider2D collider;

    private ShipGun powerWeapon;
    private float lastPingTime;

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
    private GameObject resourceArrowPrefab;

    ArrowCamera arrowCamera;

    [SerializeField]
    private ShipGun defaultWeapon;

    [SerializeField]
    private Transform shotOrigin;

    [SerializeField]
    private float dashCooldown = 2.0f;
    private float nextDashTime;
    public float dashForce = 20.0f;
    private bool shouldDash = false;

    public bool CanDock { get { return canDock; } }
    private bool canDock = false;

    public float dockTime = 1.0f;
    public float stopDockTime;

    #endregion

    private void Awake()
    {
        this.rigidbody2D = GetComponent<Rigidbody2D>();
        this.rigidbody2D.gravityScale = 0;

        this.resourceBank = GetComponentInParent<ResourceBank>();
        if (this.resourceBank == null)
            throw new System.Exception("ShipControls script requires a ResourceBank script to be present somewhere in the parent hierarchy");

        if (defaultWeapon == null)
            this.defaultWeapon = GetComponent<ShipGun>();
        if (this.defaultWeapon == null)
            throw new System.MissingFieldException("ShipControls script requires a default weapon");

        this.collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        arrowCamera = transform.parent.GetComponentInChildren<ArrowCamera>();
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

        rigidbody2D.rotation = lookAngle;
    }

    /// <summary>
    /// Ships use the Jump function to ping transmissions
    /// </summary>
    public override void Jump(ButtonState buttonState)
    {
        if (buttonState == ButtonState.WasPressed
            && Time.time - lastPingTime >= transmissionPingCooldown)
        {
            arrowCamera.StartResourcePing(transform.position, transmissionPingResourceRadius, resourceArrowPrefab);
            
            // Raise ping event
            if (OnTransmissionPing != null)
                OnTransmissionPing(this);

            lastPingTime = Time.time;
        }
    }

    public override void Shoot(ButtonState buttonState)
    {
        if (buttonState == ButtonState.IsPressed)
        {
            ShipGunShot shot;

            if (powerWeapon != null)
            {
                // Shoot power weapon & drop it
                shot = powerWeapon.Shoot(shotOrigin == null ? transform.position : shotOrigin.position, transform.up);
                powerWeapon = null;
            }
            else
            {
                shot = defaultWeapon.Shoot(shotOrigin == null ? transform.position : shotOrigin.position, transform.up);
            }
            
            if (shot != null)
                Physics2D.IgnoreCollision(shot.Collider, collider);
        }
    }

    public override void Dash(ButtonState buttonState)
    {
        if(buttonState == ButtonState.IsPressed)
        {
            shouldDash = true;
        }
    }
    #endregion

    /// <summary>
    /// Alerts this ship to the position of an enemy
    /// </summary>
    public void AlertEnemyPresence(ShipControls enemyShip)
    {
        arrowCamera.SetEnemyPing(enemyShip, resourceArrowPrefab);
    }

    private void FixedUpdate()
    {
        float movementDot = Vector2.Dot(transform.up, movementVector);
        float movementModifier = 0;

        if (movementDot >= 0)
        {
            // Map modifier to range 0.3f to 1
            movementModifier = movementDot * 0.7f + 0.3f;
        }
        else
        {
            // Map modifier to range 0.3f to 0.8f
            movementModifier = -movementDot * 0.5f + 0.3f;
        }

        Vector2 desiredVelocity = rigidbody2D.velocity + movementVector * movementModifier * impulseMagnitude * Time.fixedDeltaTime;

        if (desiredVelocity.sqrMagnitude > maximumVelocity * maximumVelocity)
            desiredVelocity = desiredVelocity.normalized * maximumVelocity;

        rigidbody2D.velocity = desiredVelocity;

        if(shouldDash)
        {
            shouldDash = false;

            if(Time.time >= nextDashTime)
            {
                nextDashTime = Time.time + dashCooldown;

                stopDockTime = Time.time + dockTime;
                canDock = true;

                rigidbody2D.AddForce(rigidbody2D.transform.up * dashForce, ForceMode2D.Impulse);
            }
        }
    }

    private void Update()
    {
        if (canDock && Time.time > stopDockTime)
            canDock = false;
    }
}
