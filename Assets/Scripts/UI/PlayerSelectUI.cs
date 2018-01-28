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

    private List<PlayerActions> playerActions = new List<PlayerActions>();

    private void Start()
    {
        startPanels[0].ShowPrompt(true);
        startPrompt.SetActive(false);

        for(int i = 0; i < startPanels.Length; i++)
        {
            int index = i;

            if (index < 0)
                index = GameManager.Instance.playerLoadouts.Length - 1;
            else if (index >= GameManager.Instance.playerLoadouts.Length)
                index = 0;

            startPanels[i].SwitchTo(index);
        }
    }

    private void Update()
    {
        for(int i = 0; i < playerActions.Count; i++)
        {
            int index = playerActions[i].loadoutIndex;
            int maxIndex = GameManager.Instance.playerLoadouts.Length - 1;

            bool pressed = false;

            if(playerActions[i].Left.WasPressed || playerActions[i].AltLeft.WasPressed)
            {
                index--;
                pressed = true;
            }
            else if (playerActions[i].Right.WasPressed || playerActions[i].AltRight.WasPressed)
            {
                index++;
                pressed = true;
            }

            if (pressed)
            {
                if (index < 0)
                    index = maxIndex;
                else if (index > maxIndex)
                    index = 0;

                startPanels[i].SwitchTo(index);

                playerActions[i].loadoutIndex = index;
            }
        }

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
            if (boundDevices[0].Action1.WasPressed)
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

                    PlayerActions action = GameManager.Instance.AddPlayerControl(device);
                    playerActions.Add(action);
                    action.loadoutIndex = currentStartPanel < GameManager.Instance.playerLoadouts.Length ? currentStartPanel : GameManager.Instance.playerLoadouts.Length - 1;

                    bound = true;
                }
            }
            else if(!addedKeyboard && Input.GetKeyDown(KeyCode.Return))
            {
                PlayerActions action = GameManager.Instance.AddPlayerControl(null);
                playerActions.Add(action);
                action.loadoutIndex = currentStartPanel < GameManager.Instance.playerLoadouts.Length ? currentStartPanel : GameManager.Instance.playerLoadouts.Length - 1;

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
