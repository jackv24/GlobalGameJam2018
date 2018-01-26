using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SplitScreenSetup : MonoBehaviour
{
    public int screenPos = 1;
    public int maxScreens = 4;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void Setup()
    {
        Rect rect = cam.rect;

        if(maxScreens == 4)
        {
            switch(screenPos)
            {
                case 1:
                    rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                    break;
                case 2:
                    rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
                case 3:
                    rect = new Rect(0, 0, 0.5f, 0.5f);
                    break;
                case 4:
                    rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    break;
            }
        }
        else if (maxScreens == 3)
        {
            switch (screenPos)
            {
                case 1:
                    rect = new Rect(0, 0, 0.5f, 1.0f);
                    break;
                case 2:
                    rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
                case 3:
                    rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    break;
            }
        }
        else if (maxScreens == 2)
        {
            switch (screenPos)
            {
                case 1:
                    rect = new Rect(0, 0, 0.5f, 1.0f);
                    break;
                case 2:
                    rect = new Rect(0.5f, 0, 0.5f, 1.0f);
                    break;
            }
        }

        cam.rect = rect;
    }
}
