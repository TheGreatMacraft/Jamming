using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject[] buttons;

    public GameObject soundMenu;

    public Sprite blueButton;
    public Sprite blueButtonHighlighted;
    
    public Sprite redButton;
    public Sprite redButtonHighlighted;
    
    public TogglePauseMenuManager togglePauseMenuManager;

    private int selectedButtonIndex = 0;

    private void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame && selectedButtonIndex > 0)
        {
            UpdateButtons(-1);
        }
        
        if(Keyboard.current.downArrowKey.wasPressedThisFrame && selectedButtonIndex < 2)
        {
            UpdateButtons(1);
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
            ButtonPressed();
    }

    private void UpdateButtons(int modifier)
    {
        SoundManager.PlaySound(SoundType.UI_MOVE);
        buttons[selectedButtonIndex].GetComponent<Image>().sprite = selectedButtonIndex == 2 ? redButton : blueButton;
        
        selectedButtonIndex += modifier;
        
        buttons[selectedButtonIndex].GetComponent<Image>().sprite = selectedButtonIndex == 2 ? redButtonHighlighted : blueButtonHighlighted;
    }

    private void ButtonPressed()
    {
        SoundManager.PlaySound(SoundType.UI_SELECT);
        switch (selectedButtonIndex)
        {
            case 0:
                togglePauseMenuManager.TogglePauseMenu();
                break;
            case 1:
                soundMenu.SetActive(true);
                this.gameObject.SetActive(false);
                break;
            case 2:
                Application.Quit();
                break;
        }
    }
}
