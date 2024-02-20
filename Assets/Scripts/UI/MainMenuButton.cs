using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    // private variables
    private GameObject GameManager;
    private TogglePauseGame togglePauseGame;
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        togglePauseGame = GetComponentInParent<TogglePauseGame>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void MainMenu()
    {
        if (!canvas.enabled) return;

        Debug.Log("Main Menu");
        togglePauseGame.PauseUnpauseGame(true);
        Destroy(GameManager);
        SceneManager.LoadScene(0); // start game loading first room
    }
}
