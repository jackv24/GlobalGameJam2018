using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class PlayerSelectUI : MonoBehaviour
{
    public float countdownTime = 10.0f;
    private float countdownTimeLeft;

    [Space()]
    public PlayerStartPanel[] startPanels;

    private int currentStartPanel = 0;

    private List<InputDevice> boundDevices = new List<InputDevice>();
    private bool addedKeyboard = false;

    [Space()]
    public GameObject startPrompt;

    private void Start()
    {
        startPanels[0].ShowPrompt(true);
        startPrompt.SetActive(false);
    }

    private void Update()
    {
        if(addedKeyboard)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameManager.Instance.StartGame();

                gameObject.SetActive(false);
            }
        }
        else if (boundDevices.Count > 0)
        {
            if (boundDevices[0].CommandWasPressed)
            {
                GameManager.Instance.StartGame();

                gameObject.SetActive(false);
            }
        }
        

        if (currentStartPanel < startPanels.Length)
        {
            bool bound = false;

            if (InputManager.ActiveDevice.Action1.WasPressed ||
                InputManager.ActiveDevice.Action2.WasPressed ||
                InputManager.ActiveDevice.Action3.WasPressed ||
                InputManager.ActiveDevice.Action4.WasPressed)
            {
                InputDevice device = InputManager.ActiveDevice;

                if (!boundDevices.Contains(device))
                {
                    boundDevices.Add(device);

                    GameManager.Instance.AddPlayerControl(device);

                    bound = true;
                }
            }
            else if(!addedKeyboard && Input.GetKeyDown(KeyCode.Return))
            {
                GameManager.Instance.AddPlayerControl(null);

                addedKeyboard = true;
                bound = true;
            }

            if(bound)
            {
                if (currentStartPanel == 1)
                    startPrompt.SetActive(true);

                startPanels[currentStartPanel].SetSelected(true);

                if (currentStartPanel < startPanels.Length - 1)
                    startPanels[currentStartPanel + 1].ShowPrompt(true);

                currentStartPanel++;
            }
        }
    }
}
