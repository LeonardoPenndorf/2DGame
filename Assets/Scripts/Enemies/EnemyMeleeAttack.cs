using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] GameObject HurtBox; // melee attack hurt box
    [SerializeField] string[] animationsArray;

    // public variables
    public int damage;
    public float knockbackForce, // determines how strong the knockback of the attack is 
                 stunDuration,  // determines how long the player is stunned by the attack
                 attackRange,
                 maxCooldown; // second the enemy needs to wait until he can attack again

    // private variables
    private Animator EnemyAnimator;
    private EnemyAggro enemyAggro;
    private AnimationChecker animationsChecker; // class containing functions to check which animtions are running
    private float cooldown;
    private int playerLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        enemyAggro = GetComponent<EnemyAggro>();
        animationsChecker = GetComponent<AnimationChecker>();

        playerLayerMask = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        if (cooldown <= 0 && enemyAggro.GetIsAggroed())
        {
            CheckRange();
        }
    }

    private void CheckRange() // check if player is within attack range
    {
        Collider2D PlayerCollider = Physics2D.OverlapCircle(transform.position, attackRange, playerLayerMask);

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
