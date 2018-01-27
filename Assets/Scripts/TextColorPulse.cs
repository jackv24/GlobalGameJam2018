using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextColorPulse : MonoBehaviour
{
    public Color minColor = Color.black;
    public Color maxColor = Color.white;

    public float speed = 1.0f;

    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if(text)
        {
            float t = Mathf.Sin(Time.time * speed);

            //Make t between 0 and 1
            t += 1;
            t *= 0.5f;

            text.color = Color.Lerp(minColor, maxColor, t);
        }
    }
}
