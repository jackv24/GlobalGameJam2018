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
                float flip = Random.Range(0, 2) == 0 ? -1 : 1;

                other.letSpawn = false;
                letSpawn = false;

                GameObject obj = Instantiate(shipInteriorPrefab, transform.parent);
                obj.transform.position = transform.position;

                Vector3 scale = obj.transform.localScale;
                scale.x *= flip;
                obj.transform.localScale = scale;

                GameObject otherObj = Instantiate(other.shipInteriorPrefab, other.transform.parent);
                otherObj.transform.position = transform.position;

                scale = otherObj.transform.localScale;
                scale.x *= -1 * flip;
                otherObj.transform.localScale = scale;

                ShipInterior interior = obj.GetComponent<ShipInterior>();
                ShipInterior otherInterior = otherObj.GetComponent<ShipInterior>();

                interior.otherShipInterior = otherInterior;
                otherInterior.otherShipInterior = interior;

                ShipInterior.DestroyEvent destroyEvent = null;
                destroyEvent = (ShipInterior self) =>
                {
                    letSpawn = true;
                    other.letSpawn = true;

                    self.OnDisabled -= destroyEvent;
                };

                interior.OnDisabled += destroyEvent;
                otherInterior.OnDisabled += destroyEvent;

                PlayerInput selfInput = GetComponentInParent<PlayerInput>();
                PlayerInput otherInput = collision.gameObject.GetComponentInParent<PlayerInput>();

                interior.owner = selfInput;
                otherInterior.owner = otherInput;

                selfInput.SwitchMode();
                otherInput.SwitchMode();
            }
        }
    }
}
