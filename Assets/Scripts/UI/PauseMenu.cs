using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InControl;

[RequireComponent(typeof(CanvasGroup))]
public class PauseMenu : MonoBehaviour
{
    public Button resumeButton;
    public Button menuButton;

    private CanvasGroup group;

    private bool paused = false;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if(resumeButton)
        {
            resumeButton.onClick.AddListener(delegate { UnPause(); });
        }

        if(menuButton)
        {
            menuButton.onClick.AddListener(delegate { LoadScene(); });
        }

        if(group)
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

    private void Update()
    {
        
    }

    public void UnPause()
    {


        paused = false;
    }

    public void Pause()
    {


        paused = true;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
