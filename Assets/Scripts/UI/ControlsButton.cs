using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControlsButton : MonoBehaviour
{
    [SerializeField] Canvas PlaystationCanvas, 
                            XboxCanvas, 
                            MouseKeyboardCanvas, 
                            PreviousCanvas, 
                            BackButtonCanvas;

    private Canvas ControlsCanvas;
    private GameObject BackButton, Controls;
    private TogglePauseGame togglePauseGame;
    private bool isEnabled = false;
    private int currentSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        BackButton = BackButtonCanvas.transform.Find("BackButton").gameObject;
        Controls = PreviousCanvas.transform.Find("ControlsButton").gameObject;

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex > 0)
            togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();
    }

    private Canvas DetectControls()
    {
        // Check for any gamepad connection
        if (Gamepad.current != null)
        {
            // Check if it's a PlayStation controller
            if (Gamepad.current is UnityEngine.InputSystem.DualShock.DualShockGamepad)
                return PlaystationCanvas;

            // Check if it's an Xbox controller
            else if (Gamepad.current is UnityEngine.InputSystem.XInput.XInputController)
                return XboxCanvas;

            else return PlaystationCanvas;
        }

        return MouseKeyboardCanvas;
    }
    
    public void ToggleControls()
    {
        if (!CheckPaused()) return;

        ControlsCanvas = DetectControls();

        if (ControlsCanvas.enabled) isEnabled = true;
        else isEnabled = false;

        ControlsCanvas.enabled = !isEnabled;
        BackButtonCanvas.enabled = !isEnabled;
        PreviousCanvas.enabled = isEnabled;

        EventSystem.current.SetSelectedGameObject(!isEnabled ? BackButton.gameObject : Controls.gameObject);
    }

    private bool CheckPaused()
    {
        if (currentSceneIndex == 0) return true;

        return togglePauseGame.GetGameIsPaused();
    }
}
