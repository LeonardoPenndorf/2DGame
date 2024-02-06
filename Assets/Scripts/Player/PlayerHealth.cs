using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // public variables
    public int maxHealth, currentHealth, damageReduction;
    public float maxIFrames;

    // private variables
    private PlayerManager playerManager; // player manager stores persistent values such as health
    private Animator animator;
    private float iFrames;
    private bool isDead = false, 
                 isBlocking = false; // when blocking takes less damage

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance; // there is only ever a single player manager instance
        animator = GetComponent<Animator>();

        currentHealth = playerManager.GetPlayerHealth(); // get current health from manager
        if (currentHealth == 0) // in the first room current health is 0
        {
            currentHealth = maxHealth;
            playerManager.AdjustPlayerHealth(maxHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = PlayerManager.instance.GetPlayerHealth(); // get the current health value

        if (iFrames > 0)
        {
            iFrames -= Time.deltaTime;
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(int damage)
    {
        if ((iFrames <= 0) && !isDead)
        {
            if (!isBlocking)
            {
                playerManager.AdjustPlayerHealth(-damage);
            }
            else
            {
                playerManager.AdjustPlayerHealth(-Mathf.Max((damage - damageReduction), 0));
            }

            iFrames = maxIFrames;

            if (currentHealth > 0)
                animator.SetTrigger("IsHit"); // trigger hit animation
        }
    }

    public void Heal(int healAmount)
    {
        playerManager.AdjustPlayerHealth(healAmount);
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
