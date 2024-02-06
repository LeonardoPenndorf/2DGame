using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    // private variables
    private GameObject GameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }

    public void MainMenu()
    {
        Debug.Log("Main Menu");
        Destroy(GameManager);
        SceneManager.LoadScene(0); // start game loading first room
    }
}
