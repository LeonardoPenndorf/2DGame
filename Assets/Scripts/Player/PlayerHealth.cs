using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // public variables
    public int maxHealth, currentHealth;
    public float maxIFrames;

    // private variables
    private Animator animator;
    private float iFrames;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

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
        if ((iFrames <= 0) && !isDead)
        {
            currentHealth -= damage;
            iFrames = maxIFrames;

            if(currentHealth > 0)
                animator.SetTrigger("IsHit"); // trigger hit animation
        }
    }

    private void Death()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<PlayerMeleeAttack>().enabled = false;
    }
}
