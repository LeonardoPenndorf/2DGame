using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCast : MonoBehaviour
{
    // public variables
    public float minDistance;

    // [SerializeField] variables
    [SerializeField] GameObject flameSkull;
    [SerializeField] float maxCooldown, interval;
    [SerializeField] int amount; // amount of flame skulls spawned

    // private variables
    private Animator animator;
    private AnimationChecker animationChecker;
    private TogglePauseGame togglePauseGame;
    private Transform spawnTransform;
    private Transform playerTransform;
    private float cooldown, distance;
    private string[] animationsArray;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animationChecker = GetComponent<AnimationChecker>();
        togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();

        spawnTransform = transform.Find("spawnPoint").transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        animationsArray = GetComponent<BossFlying>().animationsArray;

        cooldown = maxCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (togglePauseGame.GetGameIsPaused()) return;

        cooldown -= Time.deltaTime;

        if (cooldown <= 0 && !animationChecker.CheckAnimations(animationsArray))
            CheckRange();
    }

    private void CheckRange()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
        if(distance >= minDistance)
        {
            animator.SetTrigger("IsCasting");
            cooldown = maxCooldown;
        }
    }

    private IEnumerator SpawnFlameSkull() // called during the casting animation
    {
        cooldown = maxCooldown;

        for (int i = 0; i < amount; i++)
        {
            GameObject newFlameSkull = Instantiate(flameSkull, spawnTransform.position, Quaternion.identity);

            if(playerTransform.position.x - transform.position.x < 0)
                newFlameSkull.transform.rotation = Quaternion.Euler(0, 180, 0);

            yield return new WaitForSeconds(interval);
        }
    }
}
