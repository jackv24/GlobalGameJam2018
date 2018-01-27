using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInterior : MonoBehaviour
{
    public delegate void DestroyEvent(ShipInterior self);
    public event DestroyEvent OnDisabled;

    public Transform spawnPoint;

    public PlayerInput owner;

    public ShipInterior otherShipInterior;

    void OnDisable()
    {
        if(otherShipInterior)
            Destroy(otherShipInterior.gameObject);

        if (owner)
            owner.SwitchMode();

        if (OnDisabled != null)
            OnDisabled(this);
    }
}
