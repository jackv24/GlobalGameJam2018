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
            ShipInteriorSpawner other = collision.collider.GetComponent<ShipInteriorSpawner>();
            if (other && other.letSpawn)
            {
                other.letSpawn = false;
                letSpawn = false;

                GameObject obj = Instantiate(shipInteriorPrefab, transform.parent);
                obj.transform.position = transform.position;

                GameObject otherObj = Instantiate(other.shipInteriorPrefab, other.transform.parent);
                otherObj.transform.position = transform.position;

                Vector3 scale = obj.transform.localScale;
                scale.x *= -1;
                obj.transform.localScale = scale;

                ShipInterior interior = obj.GetComponent<ShipInterior>();
                ShipInterior otherInterior = otherObj.GetComponent<ShipInterior>();

                ShipInterior.DestroyEvent destroyEvent = null;
                destroyEvent = (ShipInterior self) =>
                {
                    letSpawn = true;

                    self.OnDestroyed -= destroyEvent;
                };

                interior.OnDestroyed += destroyEvent;
                otherInterior.OnDestroyed += destroyEvent;

                PlayerInput selfInput = GetComponentInParent<PlayerInput>();
                PlayerInput otherInput = collision.gameObject.GetComponentInParent<PlayerInput>();

                selfInput.SwitchMode();
                otherInput.SwitchMode();
            }
        }
    }
}
