using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleUpgradeMenu : MonoBehaviour
{
    // public variables
    public static bool gameIsPaused = false;

    // [SerializeField] variables
    [SerializeField] Canvas UpgradeMenu;

    public void OpenCloseUpgradeMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // only perform this function once

        if (gameIsPaused)
            CloseUpgradeMenu();
        else
            OpenUpgradeMenu();
    }

    public void OpenUpgradeMenu()
    {
        UpgradeMenu.enabled = true;
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void CloseUpgradeMenu()
    {
        UpgradeMenu.enabled = false;
        Time.timeScale = 1.0f;
        gameIsPaused = false;
    }
}
