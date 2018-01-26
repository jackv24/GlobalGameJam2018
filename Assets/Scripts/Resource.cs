using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    #region Fields & Properties
    [SerializeField]
    [Range(0, 100)]
    private int value;
    #endregion


    /// <summary>
    /// Consumes the resource and returns its value
    /// </summary>
    public int Consume()
    {
        // Temporary: add to resource object pool

        // Despawn this object
        Destroy(this.gameObject);

        // Return the resource value
        return value;
    }
}
