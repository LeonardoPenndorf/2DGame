using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int maxHealth, damageReduction;
    [SerializeField] float maxIFrames;

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

        InitializeHealth();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIFrames();

        CheckDeathCondition();
    }

    private void InitializeHealth()
    {
        if (playerManager.GetPlayerHealth() <= 0) // in the first room current health is 0
        {
            playerManager.SetPlayerHealth(maxHealth);
        }
    }

    private void UpdateIFrames()
    {
        if (iFrames > 0) iFrames -= Time.deltaTime;
    }

    private void CheckDeathCondition()
    {
        if (playerManager.GetPlayerHealth() <= 0 && !isDead) Death();
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

            if (playerManager.GetPlayerHealth() > 0 && !isBlocking) // don't play hit animation when dead or blocking
                animator.SetTrigger("IsHit"); // trigger hit animation
        }
    }

    public void Heal(int healAmount)
    {
        if (!isDead)
        {
            int currentHealth = playerManager.GetPlayerHealth();

            int newHealth = Mathf.Min(currentHealth + healAmount, maxHealth); // prevent overhealing

            playerManager.SetPlayerHealth(newHealth);
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
