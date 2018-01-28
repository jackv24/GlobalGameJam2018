using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGun : MonoBehaviour
{
    #region Fields & Properties
    [SerializeField]
    private ShipGunData shipGunData;

    public ShipGunData ShipGunData
    {
        get
        {
            return shipGunData;
        }
        set
        {
            if (value != null)
            {
                shipGunData = value;
                shipGunData.Validate();
            }
            else
                throw new System.ArgumentNullException();
        }
    }

    private float lastShotTime;
    #endregion

    private void Start()
    {
        if (shipGunData == null)
            throw new System.MissingFieldException("ShipGun requires shipGunData");

        shipGunData.Validate();

        lastShotTime = -ShipGunData.FireRate;
    }

    /// <summary>
    /// Shoots the gun & returns the shot instance
    /// </summary>
    public ShipGunShot Shoot(Vector2 origin, Vector2 direction)
    {
        return Shoot(origin, direction, Vector2.zero);
    }

    /// <summary>
    /// Shoots the gun & returns the shot instance
    /// </summary>
    public ShipGunShot Shoot(Vector2 origin, Vector2 direction, Vector2 extraVelocity)
    {
        if (Time.time < lastShotTime + 1 / ShipGunData.FireRateRoundsPerSecond)
            return null;

        GameObject shot = ObjectPooler.GetPooledObject(shipGunData.ShotPrefab);

        var shotBehaviour = shot.GetComponent<ShipGunShot>();

        shotBehaviour.Position = origin;
        shotBehaviour.Direction = direction;
        shotBehaviour.ExtraVelocity = extraVelocity;
        shotBehaviour.Damage = shipGunData.Damage;

        lastShotTime = Time.time;

        return shotBehaviour;
    }
}
