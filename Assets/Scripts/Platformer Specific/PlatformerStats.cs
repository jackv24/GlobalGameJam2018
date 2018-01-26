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
        StartCoroutine(DelayedInitialHealthEvent());
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
        Debug.Log(gameObject + " is dead!");
    }

    public void SendHealthEvent()
    {
        if (OnUpdateHealth != null)
            OnUpdateHealth(currentHealth, maxHealth);
    }

    IEnumerator DelayedInitialHealthEvent()
    {
        yield return null;

        SendHealthEvent();
    }
}
