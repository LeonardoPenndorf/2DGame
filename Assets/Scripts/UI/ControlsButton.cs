using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsButton : MonoBehaviour
{
    [SerializeField] Canvas ControlsCanvas, PreviousCanvas;
    [SerializeField] GameObject Back, Controls;
    private bool isEnabled = false;

    public void ToggleControls()
    {
        if (isEnabled) isEnabled = false;
        else isEnabled = true;

        ControlsCanvas.enabled = isEnabled;
        PreviousCanvas.enabled = !isEnabled;

        EventSystem.current.SetSelectedGameObject(isEnabled ? Back.gameObject : Controls.gameObject);
    }
}
