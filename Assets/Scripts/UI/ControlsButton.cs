using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ControlsButton : MonoBehaviour
{
    [SerializeField] Canvas PlaystationCanvas, MouseKeyboardCanvas, PreviousCanvas;
    [SerializeField] GameObject PlaystationBack, MouseKeyboardBack, Controls;

    private Canvas ControlsCanvas;
    private GameObject BackButton;
    private bool isEnabled = false;

    private Canvas DetectControls()
    {
        // Check for any gamepad connection
        if (Gamepad.current != null)
        {
            // Check if it's a PlayStation controller
            if (Gamepad.current is UnityEngine.InputSystem.DualShock.DualShockGamepad)
            {
                BackButton = PlaystationBack.gameObject;
                return PlaystationCanvas;
            }
            // Check if it's an Xbox controller
            else if (Gamepad.current is UnityEngine.InputSystem.XInput.XInputController)
            {
                BackButton = PlaystationBack.gameObject;
                return PlaystationCanvas;
            }

            else
            {
                BackButton = PlaystationBack.gameObject;
                return PlaystationCanvas;
            }
        }

        BackButton = MouseKeyboardBack.gameObject;
        return MouseKeyboardCanvas;
    }
    
    public void ToggleControls()
    {
        ControlsCanvas = DetectControls();

        if (ControlsCanvas.enabled) isEnabled = true;
        else isEnabled = false;

        ControlsCanvas.enabled = !isEnabled;
        PreviousCanvas.enabled = isEnabled;

        EventSystem.current.SetSelectedGameObject(!isEnabled ? BackButton.gameObject : Controls.gameObject);
    }
}
