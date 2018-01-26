using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerStats : MonoBehaviour
{
    public delegate void HealthDelegate(int currentHealth, int maxHealth);
    public event HealthDelegate OnUpdateHealth;

    public int currentHealth = 10;
    public int maxHealth = 10;

    private void Start()
    {
        SendHealthEvent();
    }

    public void RemoveHealth(int amount)
    {
        currentHealth -= Mathf.Abs(amount);

        if (currentHealth <= 0)
            Die();

        SendHealthEvent();
    }

    public void Die()
    {
        ShipInterior ship = transform.parent.GetComponentInChildren<ShipInterior>();
        if (ship)
            Destroy(ship.gameObject);

        PlayerInput player = GetComponentInParent<PlayerInput>();
        if(player)
        {
            player.DieReset();
        }

        currentHealth = maxHealth;
    }

    public void SendHealthEvent()
    {
        if (OnUpdateHealth != null)
            OnUpdateHealth(currentHealth, maxHealth);
    }
}
