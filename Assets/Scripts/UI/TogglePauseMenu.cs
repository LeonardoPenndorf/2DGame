using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TogglePauseMenu : MonoBehaviour
{
    // public variables
    public static bool gameIsPaused = false;

    // private variables
    private GameObject PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenu = GameObject.Find("PauseMenu");
    }

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
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    private void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;
    }
}
