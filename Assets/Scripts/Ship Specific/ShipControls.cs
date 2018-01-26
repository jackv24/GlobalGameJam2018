using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipControls : Controllable
{

    #region Fields & Properties
    private new Rigidbody2D rigidbody2D;
    private ResourceBank resourceBank;

    private ShipGun powerWeapon;

    private Vector2 lookVector;

    [SerializeField]
    [Range(0, 10)]
    private float impulseMagnitude = 3;

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
        ShipGun gun = worldObject.GetComponent<ShipGun>();

        if (gun == null)
            throw new System.Exception(string.Format("No Gun script found on weapon-tagged object with name {0}", worldObject.name));

        powerWeapon = gun;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.transform.tag)
        {
            case "resource":

                CollectResource(collider.transform);
                break;

            case "weapon":

                CollectWeapon(collider.transform);
                break;

            default: break;
        }
    }
    #endregion

    #region Controllable Overrides
    public override void Move(Vector2 inputVector)
    {
        rigidbody2D.AddForce(inputVector * impulseMagnitude * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    public override void Look(Vector2 inputVector)
    {
        // TODO: Slerp values?
        lookVector = inputVector;
    }

    /// <summary>
    /// Ships use the Jump function to ping transmissions
    /// </summary>
    public override void Jump(ButtonState buttonState)
    {
        if (buttonState == ButtonState.WasPressed)
        {
            // TODO: Ping that shit
        }
    }

    public override void Shoot(ButtonState buttonState)
    {
        if (lookVector.sqrMagnitude > 0 && buttonState == ButtonState.WasPressed)
        {
            if (powerWeapon != null)
            {
                // Shoot power weapon & drop it
                powerWeapon.Shoot(transform.position, lookVector);
                powerWeapon = null;
            }
            else
            {
                defaultWeapon.Shoot(transform.position, lookVector);
            }
        }
    }
    #endregion

    private void OnGUI()
    {
        GUILayout.Label("Look vector: " + lookVector.ToString());
    }
}
