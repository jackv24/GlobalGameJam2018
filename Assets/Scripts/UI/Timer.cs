using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        this.text = GetComponentInChildren<Text>();
    }

    public void StartTimer(GameManager instance)
    {
        instance.OnTimerChanged += GameManager_OnTimerChanged;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnTimerChanged -= GameManager_OnTimerChanged;
    }

    private void GameManager_OnTimerChanged(float remainingTime)
    {
        int remaining = (int)remainingTime;
        int seconds = remaining % 60;
        int minutes = remaining / 60;

        text.text = string.Format("{0}:{1}", minutes, (seconds < 10 ? ("0" + seconds) : seconds.ToString()));
    }
}
