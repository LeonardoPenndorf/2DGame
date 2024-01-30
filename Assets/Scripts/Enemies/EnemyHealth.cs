using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // public variables
    public int maxHealth, currentHealth;
    public float maxIFrames;

    // private variables
    private Animator Animator;
    private Rigidbody2D EnemyRB;
    private float iFrames;

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
        if (iFrames <= 0)
        {
            Animator.SetTrigger("IsHit"); // trigger hit animation
            currentHealth -= damage;
            iFrames = maxIFrames;
        }
    }

    private void Death()
    {
        EnemyRB.velocity = Vector3.zero;
        Animator.SetTrigger("IsDead");
        GetComponent<EnemyMeleeAttack>().enabled = false;
        GetComponent<EnemyMovement>().enabled = false;
    }
}
