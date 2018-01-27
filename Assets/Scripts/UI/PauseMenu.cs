using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InControl;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get { return instance.paused; } }

    private static PauseMenu instance;

    public Button resumeButton;
    public Button menuButton;

    private CanvasGroup group;

    private bool paused = false;

    private void Awake()
    {
        instance = this;

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

        UnPause();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || InputManager.ActiveDevice.Command.WasPressed)
        {
            if (paused)
                UnPause();
            else
                Pause();
        }
    }

    public void UnPause()
    {
        Time.timeScale = 1.0f;
        paused = false;

        if (group)
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;

            EventSystem.current.firstSelectedGameObject = null;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        paused = true;

        if (group)
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;

            GameObject firstButton = transform.GetComponentInChildren<Button>().gameObject;

            EventSystem.current.firstSelectedGameObject = firstButton;
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }

    public void LoadScene()
    {
        UnPause();

        SceneManager.LoadScene(0);
    }
}
