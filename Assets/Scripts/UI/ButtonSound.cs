using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    public AudioClip selectSound;
    public AudioClip clickSound;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.PlaySound(selectSound, transform.position);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SoundManager.PlaySound(clickSound, transform.position);
    }
}
