using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // private variables
    private GameObject GameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }

    public void Restart()
    {
        Destroy(GameManager);
        SceneManager.LoadScene(1); // start game loading first room
    }
}
