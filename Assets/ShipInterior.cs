using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInterior : MonoBehaviour
{
    public delegate void DestroyEvent(ShipInterior self);
    public event DestroyEvent OnDestroyed;

    public Transform spawnPoint;

    void OnDestroy()
    {
        if (OnDestroyed != null)
            OnDestroyed(this);
    }
}
