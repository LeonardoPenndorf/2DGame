using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckLastRoom : MonoBehaviour
{
    [SerializeField] Canvas HUDCanvas;

    // Start is called before the first frame update
    void Start()
    {
        if (IsCurrentSceneLast()) HUDCanvas.enabled = false;
    }

    private bool IsCurrentSceneLast()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        return currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1;
    }
}
