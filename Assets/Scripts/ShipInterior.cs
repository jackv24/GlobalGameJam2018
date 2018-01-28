using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInterior : MonoBehaviour
{
    public delegate void DestroyEvent(ShipInterior self);
    public event DestroyEvent OnDisabled;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

    public PlayerInput owner;

    void OnDisable()
    {
        //if(otherShipInterior)
        //    Destroy(otherShipInterior.gameObject);

        if (owner)
            owner.SwitchMode();

        if (OnDisabled != null)
            OnDisabled(this);
    }

    public void PlacePlayers(Transform player1, Transform player2)
    {
        player1.transform.position = spawnPoint1.transform.position;
        player2.transform.position = spawnPoint2.transform.position;
    }
}
