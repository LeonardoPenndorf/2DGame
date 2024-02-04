using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyBlock : MonoBehaviour
{
    // public variables
    public float defenseRange, maxCooldown;
    public string[] EnemyAnimationsArray, PlayerAnimationsArray;

    // private variables
    private Animator EnemyAnimator;
    private EnemyHealth enemyHealth;
    private AnimationChecker animationsChecker; // class containing functions to check which animations are running
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        animationsChecker = GetComponent<AnimationChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            CheckBlock();
        }
    }

    private void CheckBlock() // check if the enemy can or should block
    {
        Collider2D PlayerCollider = Physics2D.OverlapCircle(transform.position, defenseRange, LayerMask.GetMask("Player")); // check if the enemy is within the defense range
        bool preventingAnimation = animationsChecker.CheckAnimations(EnemyAnimationsArray); // check if an animation is playing, that would prevent the enemy from blocking
        bool playerAttacking = animationsChecker.CheckPlayerIsAttacking(PlayerAnimationsArray); // check if the player is attacking


        if (PlayerCollider && !preventingAnimation && playerAttacking)
        {
            EnemyAnimator.SetBool("IsBlocking", true);
            enemyHealth.SetIsBlocking(true);
        }
        else
        {
            EnemyAnimator.SetBool("IsBlocking", false);
            enemyHealth.SetIsBlocking(false);
        }
    }

    private void SetCooldown() // at the end of the block animation set block cooldown
    {
        cooldown = maxCooldown;
    }
}
