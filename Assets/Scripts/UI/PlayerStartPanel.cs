using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStartPanel : MonoBehaviour
{
    public Text pressText;
    public GameObject selectedObject;

    public AudioClip selectSound;

    private void Awake()
    {
        SetSelected(false);
        ShowPrompt(false);
    }

    public void SetSelected(bool value)
    {
        selectedObject.SetActive(value);

        if (value)
        {
            pressText.text = "Player Ready";

            if (selectSound)
                SoundManager.PlaySound(selectSound, transform.position);
        }

        ShowPrompt(!value);
    }

    public void ShowPrompt(bool value)
    {
        pressText.enabled = value;
    }
}
