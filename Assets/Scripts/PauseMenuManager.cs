using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject[] buttons;

    public Sprite blueButton;
    public Sprite blueButtonHighlighted;
    
    public Sprite redButton;
    public Sprite redButtonHighlighted;

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
    }

    private void UpdateButtons(int modifier)
    {
        buttons[selectedButtonIndex].GetComponent<SpriteRenderer>().sprite = selectedButtonIndex == 2 ? redButton : blueButton;
        
        selectedButtonIndex += modifier;
        
        buttons[selectedButtonIndex].GetComponent<SpriteRenderer>().sprite = selectedButtonIndex == 2 ? redButtonHighlighted : blueButtonHighlighted;
    }
}
