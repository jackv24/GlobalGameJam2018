using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public PlatformerStats platformerStats;
    public ShipHealth shipHealth;
    public ResourceBank resourceBank;

    public Slider shipHealthSlider;
    public Slider playerHealthSlider;
    public Text resourceText;
    public string resourceFormatString = "000000";

    private void Start()
    {
        if(platformerStats && playerHealthSlider)
        {
            platformerStats.OnUpdateHealth += (int currentHealth, int maxHealth) =>
            {
                playerHealthSlider.value = (float)currentHealth / maxHealth;
            };
        }

        if(shipHealth && shipHealthSlider)
        {
            shipHealth.OnDamage += (ShipHealth owner, int damage, int newHealthValue) =>
            {
                shipHealthSlider.value = (float)newHealthValue / owner.MaxHealth;
            };
        }

        if(resourceBank && resourceText)
        {
            resourceBank.OnResourcesChanged += (int value) => 
            {
                resourceText.text = value.ToString(resourceFormatString);
            };
        }
    }
}
