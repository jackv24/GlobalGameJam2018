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

    private void Die()
    {

    }

    void IShipDamageable.ApplyDamage(float damageValue)
    {
        currentHealth -= damageValue;

        if (currentHealth <= 0)
            Die();
    }
}
