using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGun : MonoBehaviour
{
    #region Fields & Properties
    [SerializeField]
    private ShipGunData shipGunData;

    public ShipGunData ShipGunData { get { return shipGunData; } }
    #endregion

    public void Shoot(Vector2 origin, Vector2 direction)
    {
        Debug.Log(string.Format("Firing from {0} in direction {1}", origin.ToString(), direction.ToString()));
    }
}
