using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TogglePauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject soundMenu;

    public Player playerScript;
    
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
            TogglePauseMenu();
    }

    public void TogglePauseMenu()
    {
        playerScript.enabled = !playerScript.enabled;
        Timer.Instance.enabled = !Timer.Instance.enabled;
        
        if (soundMenu.activeSelf)
        {
            soundMenu.SetActive(false);
            return;
        }
        
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
}
