using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRotation : MonoBehaviour
{
    public float amount;

    private void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z += amount * Time.deltaTime;
        transform.eulerAngles = rotation;
    }
}
