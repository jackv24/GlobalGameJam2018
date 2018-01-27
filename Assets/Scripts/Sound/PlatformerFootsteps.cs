using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerFootsteps : MonoBehaviour
{
    public AudioEvent footstepSound;

    public void PlayFootstep()
    {
        footstepSound.Play(transform.position);
    }
}
