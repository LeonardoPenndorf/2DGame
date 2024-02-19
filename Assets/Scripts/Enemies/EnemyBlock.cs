using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyBlock : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] float defenseRange, maxCooldown, blockDuration;
    [SerializeField] string[] EnemyAnimationsArray, PlayerAnimationsArray;

    // private variables
    private Animator EnemyAnimator;
    private EnemyHealth enemyHealth;
    private AnimationChecker animationsChecker; // class containing functions to check which animations are running
    private EnemyMovement enemyMovement;
    private Transform Player;
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        animationsChecker = GetComponent<AnimationChecker>();
        enemyMovement = GetComponent<EnemyMovement>();

        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            CheckBlock();
        }
    }

    private void CheckBlock() // check if the enemy can or should block
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (distanceToPlayer <= defenseRange && CanBlock() && enemyMovement.GetCanAttack())
        {
            StartCoroutine(Blocking());
        }
    }

    // check if there is an atack the enemy can block and if he's already performing an animation that would prevent him from blocking
    private bool CanBlock()
    {
        return !animationsChecker.CheckAnimations(EnemyAnimationsArray) && animationsChecker.CheckPlayerIsAttacking(PlayerAnimationsArray);
    }

    private IEnumerator Blocking()
    {
        EnemyAnimator.SetBool("IsBlocking", true);
        enemyHealth.SetIsBlocking(true);

        yield return new WaitForSeconds(blockDuration);

        EnemyAnimator.SetBool("IsBlocking", false);
        enemyHealth.SetIsBlocking(false);

        cooldown = maxCooldown; // wait until the cooldown has expired before you can block again
    }
}
