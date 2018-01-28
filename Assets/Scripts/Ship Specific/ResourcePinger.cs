using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePinger : MonoBehaviour
{
    public delegate void PingerDespawnDelegate(ResourcePinger source);
    public event PingerDespawnDelegate OnDespawn;

    public ParticleSystem particlePrefab;

    private void Start()
    {
        ParticleSystem system = Instantiate<ParticleSystem>(particlePrefab, this.transform);
        system.transform.localPosition = new Vector3(0, 0, 0.01f);
    }

    private void OnDisable()
    {
        if (OnDespawn != null)
            OnDespawn(this);
    }
}
