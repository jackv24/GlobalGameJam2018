using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerStats : MonoBehaviour
{
    public delegate void HealthDelegate(int currentHealth, int maxHealth);
    public event HealthDelegate OnUpdateHealth;

    public int currentHealth = 10;
    public int maxHealth = 10;

    private PlatformerAnimation anim;

    private Coroutine dieRoutine = null;

    private void Awake()
    {
        anim = GetComponent<PlatformerAnimation>();
    }

    private void OnEnable()
    {
        anim.isAlive = true;
    }

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
        anim.isAlive = false;

        if(dieRoutine == null)
            dieRoutine = StartCoroutine(DieDelayed(3.0f));
    }

    public void SendHealthEvent()
    {
        if (OnUpdateHealth != null)
            OnUpdateHealth(currentHealth, maxHealth);
    }

    IEnumerator DieDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        ShipInterior ship = transform.root.GetComponentInChildren<ShipInterior>();

        if (ship)
            Destroy(ship.gameObject);

        PlayerInput player = GetComponentInParent<PlayerInput>();
        if (player)
        {
            player.DieReset();
        }

        currentHealth = maxHealth;

        SendHealthEvent();

        dieRoutine = null;
    }
}
