using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SoundMenuManager : MonoBehaviour
{
    public GameObject[] sliders;
    public AudioSource[] audioSources;
    
    public Image backButton;

    public GameObject pauseMenu;

    public Sprite background;
    public Sprite fill;
    
    public Sprite backgroundHighlighted;
    public Sprite fillHighlighted;
    
    public Sprite redButton;
    public Sprite redButtonHighlighted;

    private int selectedElementIndex = 0;
    
    //Glupost, ampak dela
    private int frameCounter = 0;

    public int checkSliderEveryXFrame = 10;

    private void Update()
    {
        frameCounter++;
        
        if (Keyboard.current.upArrowKey.wasPressedThisFrame && selectedElementIndex > 0)
        {
            UpdateElements(-1);
        }
        
        if(Keyboard.current.downArrowKey.wasPressedThisFrame && selectedElementIndex < 2)
        {
            UpdateElements(1);
        }

        if (selectedElementIndex == 2 && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            pauseMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }

        if (selectedElementIndex != 2 && frameCounter % checkSliderEveryXFrame == 0)
        {
            if (Keyboard.current.leftArrowKey.isPressed)
                ButtonPressed(-1);
            if (Keyboard.current.rightArrowKey.isPressed)
                ButtonPressed(1);
        }
    }

    private void UpdateElements(int modifier)
    {
        if (selectedElementIndex == 2)
            backButton.sprite = redButton;
        else
        {
            sliders[selectedElementIndex].transform.Find("Background").GetComponent<Image>().sprite = background;
            sliders[selectedElementIndex].transform.Find("Fill").GetComponent<Image>().sprite = fill;
        }
        
        selectedElementIndex += modifier;
        
        if (selectedElementIndex == 2)
            backButton.sprite = redButtonHighlighted;
        else
        {
            sliders[selectedElementIndex].transform.Find("Background").GetComponent<Image>().sprite = backgroundHighlighted;
            sliders[selectedElementIndex].transform.Find("Fill").GetComponent<Image>().sprite = fillHighlighted;
        }
        
    }

    private void ButtonPressed(int modifier)
    {
        Vector3 scale = sliders[selectedElementIndex].transform.Find("Fill").transform.localScale;
        
        float newScale = scale.x + modifier;

        if (newScale >= 0 && newScale <= 100)
            scale.x = newScale;
        
        audioSources[selectedElementIndex].volume = scale.x/100;
        sliders[selectedElementIndex].transform.Find("Fill").transform.localScale = scale;
    }
}
