using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // public variables
    public int maxHealth, currentHealth;
    public float maxIFrames;

    // private variables
    private Animator animator;
    private float iFrames;

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

        if(currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamge(int damage)
    {
        if (iFrames <= 0)
        {
            animator.SetTrigger("IsHit"); // trigger hit animation
            currentHealth -= damage;
            iFrames = maxIFrames;
        }
    }

    private void Death()
    {
        Destroy(gameObject); // placeholder
    }
}
