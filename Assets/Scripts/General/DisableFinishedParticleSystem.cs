using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DisableFinishedParticleSystem : MonoBehaviour
{
    private ParticleSystem system;

    private void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!system.isEmitting && system.particleCount <= 0)
            gameObject.SetActive(false);
    }
}
