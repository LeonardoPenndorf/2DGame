using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // public variables
    public int maxHealth, currentHealth, damageReduction;
    public float maxIFrames;

    // private variables
    private Animator Animator;
    private Rigidbody2D EnemyRB;
    private float iFrames;
    private bool isBlocking = false; // some enemies can block


    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        EnemyRB = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
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
            Animator.SetTrigger("IsHit"); // trigger hit animation

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
        Animator.SetTrigger("IsDead");
        GetComponent<EnemyMeleeAttack>().enabled = false;
        GetComponent<EnemyMovement>().enabled = false;

        EnemyBlock EnemyBlockComponent = GetComponent<EnemyBlock>(); // check if the enemy has the block component
        if(EnemyBlockComponent != null)
        {
            GetComponent<EnemyBlock>().enabled = false;
        }
    }

    public void SetIsBlocking(bool newState)
    {
        isBlocking = newState;
    }
}
