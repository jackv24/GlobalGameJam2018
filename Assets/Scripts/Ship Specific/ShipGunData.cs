using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipGunData.asset", menuName = "Ship Gun Data", order = 0)]
public class ShipGunData : ScriptableObject
{
    public GameObject ShotPrefab;

    public int Damage;

    [Tooltip("Fire Rate in Rounds Per Minute")]
    public float FireRate;

    public float FireRateRoundsPerSecond { get { return FireRate / 60; } }

    public void Validate()
    {
        if (ShotPrefab == null)
            throw new System.MissingFieldException("ShipGunData requires ShotPrefab");

        if (ShotPrefab.GetComponent<ShipGunShot>() == null)
            throw new System.Exception("ShipGunData ShotPrefab must have a ShipGunShot component");
    }
}
