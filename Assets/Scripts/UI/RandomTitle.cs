using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RandomTitle : MonoBehaviour
{
    public static bool seenOnce = false;

    public string[] alternateTitles;

    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        if (!seenOnce)
            seenOnce = true;
        else
        {
            if(text)
            {
                text.text = alternateTitles[Random.Range(0, alternateTitles.Length)];
            }
        }
    }
}
