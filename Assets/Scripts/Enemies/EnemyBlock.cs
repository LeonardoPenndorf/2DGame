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
    private Animator EnemyAnimator, PlayerAnimator;
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        PlayerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
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
            CheckRange();
        }
    }

    private void CheckRange() // check if player is within defense range
    {
        Collider2D PlayerCollider = Physics2D.OverlapCircle(transform.position, defenseRange, LayerMask.GetMask("Player"));

        if (PlayerCollider && !CheckEnemyAnimations() && CheckPlayerIsAttacking())
        {
            EnemyAnimator.SetBool("IsBlocking", true);
            gameObject.GetComponent<EnemyHealth>().SetIsBlocking(true);
        }
        else
        {
            EnemyAnimator.SetBool("IsBlocking", false);
            gameObject.GetComponent<EnemyHealth>().SetIsBlocking(false);
        }
    }

    private bool CheckEnemyAnimations() // check if an animation is playing, which would prevent the enemy from blocking
    {
        AnimatorStateInfo stateInfo = EnemyAnimator.GetCurrentAnimatorStateInfo(0);

        foreach (string animationName in EnemyAnimationsArray)
        {
            if (stateInfo.IsName(animationName))
            {
                return true; // Return true if an animation in the array is currently playing
            }
        }

        return false;
    }

    private bool CheckPlayerIsAttacking() // check if the player is attacking
    {
        AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        foreach (string animationName in PlayerAnimationsArray)
        {
            if (stateInfo.IsName(animationName))
            {
                return true; // Return true if an attack animation is currently playing
            }
        }

        return false;
    }

    private void SetCooldown() // at the end of the block animation set block cooldown
    {
        cooldown = maxCooldown;
    }
}
