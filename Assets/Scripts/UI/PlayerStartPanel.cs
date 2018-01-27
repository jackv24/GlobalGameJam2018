using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStartPanel : MonoBehaviour
{
    public Text pressText;
    public GameObject selectedObject;

    public AudioClip selectSound;

    [Space()]
    public RectTransform profileContainer;
    public float[] profileStopPoint = new float[] { 0, -960, -1920 };
    public float lerpTime = 0.25f;

    private Coroutine animRoutine;

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

    public void SwitchTo(int index)
    {
        if (animRoutine != null)
            StopCoroutine(animRoutine);

        animRoutine = StartCoroutine(SwitchToAnimation(index));
    }

    IEnumerator SwitchToAnimation(int index)
    {
        float currentStopPoint = profileContainer.localPosition.x;
        float nextStopPoint = profileStopPoint[index];

        float elapsed = 0;
        while(elapsed <= lerpTime)
        {
            Vector3 pos = profileContainer.anchoredPosition;
            pos.x = Mathf.Lerp(currentStopPoint, nextStopPoint, elapsed / lerpTime);
            profileContainer.anchoredPosition = pos;

            yield return null;
            elapsed += Time.deltaTime;
        }

        {
            Vector3 pos = profileContainer.anchoredPosition;
            pos.x = nextStopPoint;
            profileContainer.anchoredPosition = pos;
        }

        animRoutine = null;
    }
}
