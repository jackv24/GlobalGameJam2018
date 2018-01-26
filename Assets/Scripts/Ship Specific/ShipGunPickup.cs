using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGunPickup : MonoBehaviour
{
    [SerializeField]
    private ShipGunData data;

    /// <summary>
    /// Consumes the pickup and returns the Data
    /// </summary>
    public ShipGunData Consume()
    {
        Destroy(gameObject);

        return data;
    }
}
