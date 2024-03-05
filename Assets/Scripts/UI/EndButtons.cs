using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButtons : MonoBehaviour
{

    private GameObject GameManager;
    private TogglePauseGame togglePauseGame;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();
    }

    public void Restart()
    {
        togglePauseGame.PauseUnpauseGame(true);
        Destroy(GameManager);
        SceneManager.LoadScene(1); // start game loading first room
    }

    public void MainMenu()
    {
        togglePauseGame.PauseUnpauseGame(true);
        Destroy(GameManager);
        SceneManager.LoadScene(0); // start game loading first room
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
