using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // public variables
    public int maxHealth, currentHealth, damageReduction;
    public float maxIFrames;

    // private variables
    private Animator EnemyAnimator;
    private Rigidbody2D EnemyRB;
    private float iFrames;
    private bool isBlocking = false; // some enemies can block


    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        EnemyRB = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        EnemyManager.instance.RegisterEnemy(gameObject); // add enemy to the enemies list in enemy manager
    }

    // Update is called once per frame
    void Update()
    {
        if (iFrames > 0)
        {
            iFrames -= Time.deltaTime;
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamge(int damage)
    {
        if (iFrames <= 0 && currentHealth > 0)
        {
            EnemyAnimator.SetTrigger("IsHit"); // trigger hit animation

            if (!isBlocking)
            {
                currentHealth -= damage;
            }
            else // when blocking subtract damage reduction from damage taken
            {
                currentHealth -= Mathf.Max((damage - damageReduction), 0);
            }

            iFrames = maxIFrames;
        }
    }

    private void Death()
    {
        EnemyRB.velocity = Vector3.zero;
        EnemyAnimator.SetTrigger("IsDead");
        GetComponent<EnemyMeleeAttack>().enabled = false;

        EnemyMovement EnemyMovementComponent = GetComponent<EnemyMovement>();
        if(EnemyMovementComponent != null)
        {
            GetComponent<EnemyMovement>().enabled = false;
        }

        RangedEnemyMovement RangedEnemyMovementComponent = GetComponent<RangedEnemyMovement>();
        if (RangedEnemyMovementComponent != null)
        {
            GetComponent<RangedEnemyMovement>().enabled = false;
        }

        EnemyBlock EnemyBlockComponent = GetComponent<EnemyBlock>(); // check if the enemy has the block component
        if(EnemyBlockComponent != null)
        {
            GetComponent<EnemyBlock>().enabled = false;
            EnemyAnimator.SetBool("IsBlocking", false);
        }

        EnemyManager.instance.EnemyDied(gameObject); // remove enemy from the enemies list in enemy manager
    }

    public void SetIsBlocking(bool newState)
    {
        isBlocking = newState;
    }
}
