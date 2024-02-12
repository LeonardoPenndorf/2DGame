using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    // private variables
    private GameObject GameManager;
    private TogglePauseMenu togglePauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        togglePauseMenu = GetComponentInParent<TogglePauseMenu>();
    }

    public void MainMenu()
    {
        togglePauseMenu.ResumeGame();
        Destroy(GameManager);
        SceneManager.LoadScene(0); // start game loading first room
    }
}
