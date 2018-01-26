using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBank : MonoBehaviour
{
    private int resources;

    public void AddResources(Resource resource)
    {
        resources += resource.Consume();
    }
}
