using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBank : MonoBehaviour
{
    public delegate void ResourceChangeEvent(int resourceCount);
    public event ResourceChangeEvent OnResourcesChanged;

    private int resources;

    public void AddResources(Resource resource)
    {
        resources += resource.Consume();

        if (OnResourcesChanged != null)
            OnResourcesChanged(resources);
    }
}
