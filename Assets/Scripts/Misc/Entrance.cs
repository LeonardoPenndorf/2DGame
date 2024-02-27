using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    [SerializeField] float timer;

    private Animator doorAnimator, playerAnimator;
    private Transform spawnPoint;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>();
        spawnPoint = transform.Find("SpawnPoint").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();

        player.transform.position = spawnPoint.transform.position;

        StartCoroutine(Enter());
    }

    private IEnumerator Enter()
    {
        yield return new WaitForSeconds(timer);
        playerAnimator.SetTrigger("Enter");
        yield return new WaitForSeconds(timer);
        doorAnimator.SetTrigger("Close");
    }
}
