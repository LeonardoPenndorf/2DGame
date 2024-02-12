using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TogglePauseMenu : MonoBehaviour
{
    // public variables
    public static bool gameIsPaused = false;

    // [SerializeField] variables
    [SerializeField] Canvas PauseMenu;

    public void PauseUnpauseGame(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // only perform this function once

        if (gameIsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        PauseMenu.enabled = true;
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        PauseMenu.enabled = false;
        Time.timeScale = 1.0f;
        gameIsPaused = false;
    }
}
