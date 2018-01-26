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

    private void Start()
    {
        startPanels[0].ShowPrompt(true);
    }

    private void Update()
    {
        if (currentStartPanel < startPanels.Length)
        {
            if (InputManager.ActiveDevice.AnyButtonWasPressed)
            {
                InputDevice device = InputManager.ActiveDevice;

                if (!boundDevices.Contains(device))
                {
                    boundDevices.Add(device);

                    GameManager.Instance.AddPlayerControl(device);

                    startPanels[currentStartPanel].SetSelected(true);

                    if (currentStartPanel < startPanels.Length - 1)
                        startPanels[currentStartPanel + 1].ShowPrompt(true);

                    currentStartPanel++;
                }
            }
        }

        if(boundDevices.Count > 0)
        {
            if(boundDevices[0].CommandWasPressed)
            {
                GameManager.Instance.SpawnPlayers();

                gameObject.SetActive(false);
            }
        }
    }
}
