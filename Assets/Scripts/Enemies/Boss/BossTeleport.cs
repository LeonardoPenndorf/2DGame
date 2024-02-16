using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleport : MonoBehaviour
{
    [SerializeField] float maxCooldown, teleportTime;

    // private variables
    private BoxCollider2D bossCollider;
    private BossFlying bossFlying;
    private Animator animator;
    private AnimationChecker animationChecker;
    private Transform aimTransform;
    private float cooldown;
    private string[] animationsArray;
    private bool isPresent = true;

    // Start is called before the first frame update
    void Start()
    {
        bossCollider = GetComponent<BoxCollider2D>();
        bossFlying = GetComponent<BossFlying>();

        animator = GetComponent<Animator>();
        animationChecker = GetComponent<AnimationChecker>();

        aimTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("AimPoint").transform;

        animationsArray = GetComponent<BossFlying>().animationsArray;

        cooldown = maxCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        if (cooldown <= 0 && !animationChecker.CheckAnimations(animationsArray) && isPresent)
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        isPresent = false;

        animator.SetTrigger("IsTeleporting");
        bossCollider.enabled = false;
        bossFlying.enabled = false;

        StartCoroutine(Reappear());
    }

    private IEnumerator Reappear()
    {
        yield return new WaitForSeconds(teleportTime);

        transform.position = aimTransform.position;

        animator.SetTrigger("IsAttacking");

        bossCollider.enabled = true;
        bossFlying.enabled = true;
        isPresent = true;

        cooldown = maxCooldown;
    }
}
