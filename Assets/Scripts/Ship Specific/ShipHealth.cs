using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour, IShipDamageable
{

    #region Fields & Properties
    private float currentHealth;

    [SerializeField]
    [Range(1, 100)]
    private float startingHealthValue;
    #endregion

    private void Awake()
    {
        currentHealth = startingHealthValue;
    }

    void IShipDamageable.ApplyDamage(float damageValue)
    {
        currentHealth -= damageValue;
    }
}
