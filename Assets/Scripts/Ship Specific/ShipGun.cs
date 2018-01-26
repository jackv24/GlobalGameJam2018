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
    #endregion

    private void Start()
    {
        if (shipGunData == null)
            throw new System.MissingFieldException("ShipGun requires shipGunData");

        shipGunData.Validate();
    }

    /// <summary>
    /// Shoots the gun & returns the shot instance
    /// </summary>
    public ShipGunShot Shoot(Vector2 origin, Vector2 direction)
    {
        GameObject shot = ObjectPooler.GetPooledObject(shipGunData.ShotPrefab);

        var shotBehaviour = shot.GetComponent<ShipGunShot>();

        shotBehaviour.Position = origin;
        shotBehaviour.Direction = direction;
        shotBehaviour.Damage = shipGunData.Damage;

        return shotBehaviour;
    }
}
