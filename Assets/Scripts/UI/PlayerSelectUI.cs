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

    private void Start()
    {
        startPanels[0].ShowPrompt(true);
    }

    private void Update()
    {
        if(addedKeyboard)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameManager.Instance.SpawnPlayers();

                gameObject.SetActive(false);
            }
        }
        else if (boundDevices.Count > 0)
        {
            if (boundDevices[0].CommandWasPressed)
            {
                GameManager.Instance.SpawnPlayers();

                gameObject.SetActive(false);
            }
        }
        

        if (currentStartPanel < startPanels.Length)
        {
            bool bound = false;

            if (InputManager.ActiveDevice.AnyButtonWasPressed)
            {
                InputDevice device = InputManager.ActiveDevice;

                if (!boundDevices.Contains(device))
                {
                    boundDevices.Add(device);

                    GameManager.Instance.AddPlayerControl(device);

                    bound = true;
                }
            }
            else if(!addedKeyboard && Input.anyKeyDown)
            {
                GameManager.Instance.AddPlayerControl(null);

                addedKeyboard = true;
                bound = true;
            }

            if(bound)
            {
                startPanels[currentStartPanel].SetSelected(true);

                if (currentStartPanel < startPanels.Length - 1)
                    startPanels[currentStartPanel + 1].ShowPrompt(true);

                currentStartPanel++;
            }
        }
    }
}
