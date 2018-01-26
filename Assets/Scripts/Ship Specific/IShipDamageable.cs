using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShipDamageable
{
    void ApplyDamage(int damageValue);

    int CurrentHealth { get; }
}
