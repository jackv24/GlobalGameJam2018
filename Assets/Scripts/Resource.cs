using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    #region Fields & Properties
    [SerializeField]
    [Range(0, 100)]
    private int value;

    public int Value
    {
        get { return value; }
        set { this.value = value; }
    }
    #endregion


    /// <summary>
    /// Consumes the resource and returns its value
    /// </summary>
    public int Consume()
    {
        // Despawn this object
        this.gameObject.SetActive(false);

        // Return the resource value
        return value;
    }
}
