using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePinger : MonoBehaviour
{
    public delegate void PingerDespawnDelegate(ResourcePinger source);
    public event PingerDespawnDelegate OnDespawn;

    private void OnDisable()
    {
        if (OnDespawn != null)
            OnDespawn(this);
    }
}
