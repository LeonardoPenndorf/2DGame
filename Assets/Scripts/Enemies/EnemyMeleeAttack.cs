using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    // public variables
    public int damage;
    public float knockbackForce, // determines how satrong the knockback of the attack is 
                 stunDuration,  // determines how long the player is stunned by the attack
                 attackRange,
                 maxCooldown; // second the enemy needs to wait until he can attack again
    public GameObject HurtBox; // melee attack hurt box
    public string[] animationsArray;


    // private variables
    private Animator EnemyAnimator;
    private AnimationChecker animationsChecker; // class containing functions to check which animtions are running
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
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
            CheckRange(); // if attack is of cooldown, check range
        }

    }

    private void CheckRange() // check if player is within attack range
    {
        Collider2D PlayerCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));

        if (PlayerCollider && !animationsChecker.CheckAnimations(animationsArray))
        {
            MeleeAttack(); // if player is within attack range, attack   
        }
    }

    private void MeleeAttack()
    {
        EnemyAnimator.SetTrigger("IsAttacking");
        cooldown = maxCooldown;
    }

    public void enableHurtboxCollider() // enable hurtbox collider at beginning of animation
    {
        HurtBox.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void disableHurtboxCollider() // disable hurtbox collider at end of animation
    {
        HurtBox.GetComponent<BoxCollider2D>().enabled = false;
    }
}
