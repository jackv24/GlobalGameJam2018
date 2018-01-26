using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SplitScreenSetup : MonoBehaviour
{
    public int screenPos = 1;
    public int maxScreens = 4;

    public float edgePadding = 0.05f;

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

        if(rect != cam.rect)
        {
            float padding = edgePadding / 2;
            float ratio = (float)Screen.height / Screen.width;

            Vector2 size = rect.size;
            size.x -= edgePadding * ratio;
            size.y -= edgePadding;
            rect.size = size;

            Vector2 pos = rect.position;
            pos.x += padding * ratio;
            pos.y += padding;
            rect.position = pos;
        }

        cam.rect = rect;
    }
}
