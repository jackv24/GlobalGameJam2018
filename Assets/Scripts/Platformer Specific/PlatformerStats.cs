using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerStats : MonoBehaviour
{
    public int currentHealth = 10;
    public int maxHealth = 10;

    public void RemoveHealth(int amount)
    {
        currentHealth -= Mathf.Abs(amount);

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        Debug.Log(gameObject + " is dead!");
    }
}
