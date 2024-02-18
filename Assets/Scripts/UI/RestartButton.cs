using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // private variables
    private GameObject GameManager;
    private TogglePauseGame togglePauseGame;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        togglePauseGame = GetComponentInParent<TogglePauseGame>();
    }

    public void Restart()
    {
        togglePauseGame.PauseUnpauseGame(true);
        Destroy(GameManager);
        SceneManager.LoadScene(1); // start game loading first room
    }
}
