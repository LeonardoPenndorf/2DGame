using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TogglePauseGame : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] Canvas PauseMenu;
    [SerializeField] Canvas UpgradeMenu;

    // private variables
    private UpgradeSystem upgradeSystem;
    private bool gameIsPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = UpgradeMenu.GetComponent<UpgradeSystem>();
    }

    public bool GetGameIsPaused() { return gameIsPaused; }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (UpgradeMenu.enabled || !context.performed) return;

        PauseMenu.enabled = !PauseMenu.enabled;
        PauseUnpauseGame(gameIsPaused);
    }

    public void ToggleUpgradeMenu(InputAction.CallbackContext context)
    {
        if (PauseMenu.enabled || !context.performed) return;

        upgradeSystem.UpdateDescription();
        UpgradeMenu.enabled = !UpgradeMenu.enabled;
        PauseUnpauseGame(gameIsPaused);
    }

    public void PauseUnpauseGame(bool state)
    {
        if (state)
        {
            Time.timeScale = 1.0f;
            gameIsPaused = false;
        }
        else
        {
            Time.timeScale = 0f;
            gameIsPaused = true;
        }
    }

    public void UnpauseGame()
    {
        PauseMenu.enabled = false;
        PauseUnpauseGame(true);
    }
}
