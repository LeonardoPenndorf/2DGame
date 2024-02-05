using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    // public variables
    public float loadingTime;

    // private variables
    private Animator DoorAnimator;
    private int currentRoomIndex;
    private bool allEnemiesDead = false;

    // Start is called before the first frame update
    void Start()
    {
        DoorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemies();
    }

    public void CheckEnemies() // check if all enemies are dead using the enemy manager
    {
        if (EnemyManager.instance.AreAllEnemiesDead())
        {
            DoorAnimator.SetTrigger("Open");
            allEnemiesDead = true;
        }
    }

    public void StartLoadingRoom()
    {
        if (allEnemiesDead) // you can only leave when all enemies are dead
        {
            DoorAnimator.SetTrigger("Close");

            StartCoroutine(LoadRoom());
        }
    }

    IEnumerator LoadRoom()
    {
        yield return new WaitForSeconds(loadingTime);

        currentRoomIndex = SceneManager.GetActiveScene().buildIndex; // get current scene

        SceneManager.LoadScene(currentRoomIndex + 1); // Load next scene
    }
}
