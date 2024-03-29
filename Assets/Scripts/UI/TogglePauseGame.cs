using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TogglePauseGame : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] Canvas PauseMenu, UpgradeMenu, PlyastationControls, MouseKeyboardControls, XboxControls, Backbutton;
    [SerializeField] GameObject ContinueButton, UpgradeDamageButton;

    // private variables
    private PlayerManager playerManager; // player manager stores persistent values such as health
    private UpgradeSystem upgradeSystem;
    private EventSystem eventSystem;
    private bool gameIsPaused = false, 
                 lastScene = false;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance; // there is only ever a single player manager instance
        upgradeSystem = UpgradeMenu.GetComponent<UpgradeSystem>();
        eventSystem = EventSystem.current;

        lastScene = IsCurrentSceneLast();
    }

    public bool GetGameIsPaused() { return gameIsPaused; }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (lastScene || UpgradeMenu.enabled || !context.performed || playerManager.GetPlayerHealth() <= 0) return;

        if (PlyastationControls.enabled || MouseKeyboardControls.enabled || XboxControls.enabled)
        {
            PlyastationControls.enabled = false;
            MouseKeyboardControls.enabled = false;
            XboxControls.enabled = false;
            Backbutton.enabled = false;
        }
        else
        {
            PauseMenu.enabled = !PauseMenu.enabled;
            eventSystem.SetSelectedGameObject(ContinueButton.gameObject);
        }

        PauseUnpauseGame(gameIsPaused);
    }

    public void ToggleUpgradeMenu(InputAction.CallbackContext context)
    {
        if (lastScene || PauseMenu.enabled || PlyastationControls.enabled || MouseKeyboardControls.enabled || XboxControls.enabled || !context.performed || playerManager.GetPlayerHealth() <= 0) return;

        upgradeSystem.UpdateDescription();
        UpgradeMenu.enabled = !UpgradeMenu.enabled;
        PauseUnpauseGame(gameIsPaused);
        eventSystem.SetSelectedGameObject(UpgradeDamageButton.gameObject);
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

    private bool IsCurrentSceneLast()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        return currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1;
    }
}
