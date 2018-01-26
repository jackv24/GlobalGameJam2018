using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour, IShipDamageable
{

    #region Delegates & Events
    public delegate void ShipHealthDelegate(ShipHealth owner, int damage, int newHealthValue);

    public event ShipHealthDelegate OnDamage;
    #endregion

    #region Fields & Properties
    private int currentHealth;

    [SerializeField]
    [Range(1, 100)]
    private int maxHealth;
    public int MaxHealth { get { return maxHealth; } }
    #endregion

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        if (OnDamage != null)
            OnDamage(this, 0, currentHealth);
    }

    public void Die(bool dieReset = true)
    {
        if (dieReset)
        {
            PlayerInput player = GetComponentInParent<PlayerInput>();
            if (player)
            {
                player.DieReset(false);
            }
        }

        //TODO: Explosions and shit
        currentHealth = maxHealth;
    }

    int IShipDamageable.CurrentHealth { get { return currentHealth; } }

    void IShipDamageable.ApplyDamage(int damageValue)
    {
        currentHealth -= damageValue;

        if (currentHealth < 0)
            currentHealth = 0;

        if (OnDamage != null)
            OnDamage(this, damageValue, currentHealth);

        if (currentHealth <= 0)
            Die();
    }
}
