using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // public variables
    public int maxHealth, currentHealth, damageReduction;
    public float maxIFrames;

    // private variables
    private Animator animator;
    private float iFrames;
    private bool isDead = false, 
                 isBlocking = false; // when blocking takes less damage

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
            if (!isBlocking)
            {
                currentHealth -= damage;
                Debug.Log("not blocking");
            }
            else
            {
                currentHealth -= Mathf.Max((damage - damageReduction), 0);
            }

            iFrames = maxIFrames;

            if(currentHealth > 0)
                animator.SetTrigger("IsHit"); // trigger hit animation
        }
    }

    public void SetIsBlocking(bool newIsBlocking)
    {
        isBlocking = newIsBlocking;
    }

    private void Death()
    {
        isDead = true;
        animator.SetTrigger("IsDead");
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<PlayerMeleeAttack>().enabled = false;
    }
}
