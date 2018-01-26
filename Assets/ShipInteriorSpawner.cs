using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInteriorSpawner : MonoBehaviour
{
    public GameObject shipInteriorPrefab;
    
    [HideInInspector]
    public bool letSpawn = true;

    private void OnEnable()
    {
        letSpawn = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (letSpawn)
        {
            GameObject obj = Instantiate(shipInteriorPrefab, transform.parent);
            obj.transform.position = transform.position;

            ShipInteriorSpawner other = collision.collider.GetComponent<ShipInteriorSpawner>();
            if (other)
            {
                other.letSpawn = false;

                GameObject otherObj = Instantiate(other.shipInteriorPrefab, other.transform.parent);
                otherObj.transform.position = transform.position;

                Vector3 scale = obj.transform.localScale;
                scale.x *= -1;
                obj.transform.localScale = scale;
            }

            PlayerInput selfInput = GetComponentInParent<PlayerInput>();
            PlayerInput otherInput = collision.gameObject.GetComponentInParent<PlayerInput>();

            selfInput.SwitchMode();
            otherInput.SwitchMode();
        }
    }
}
