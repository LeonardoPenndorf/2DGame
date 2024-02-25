using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    // private variables
    private PlayerManager playerManager; // player manager stores persistent values such as health
    private PlayerBlock playerBlock;
    private Animator animator;
    private PlayerInput playerInput; // upon death switch input mapto disable input
    private bool isDead = false, 
                 isBlocking = false; // when blocking takes less damage
    private int playerlayerMask, 
                invLayermask;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance; // there is only ever a single player manager instance
        playerBlock = GetComponent<PlayerBlock>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        playerlayerMask = gameObject.layer;
        invLayermask = LayerMask.NameToLayer("Player_Inv");
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeathCondition();
    }

    private void CheckDeathCondition()
    {
        if (playerManager.GetPlayerHealth() <= 0 && !isDead) Death();
    }

    public void TakeDamage(int damage, Transform hurtboxTransform)
    {
        if (isDead || (isBlocking && playerBlock.IsAttackComingFromFront(hurtboxTransform))) { return; }

        playerManager.AdjustPlayerHealth(-damage);

        if (playerManager.GetPlayerHealth() > 0) animator.SetTrigger("IsHit");
    }

    public void Heal(int healAmount)
    {
        if (!isDead)
        {
            int currentHealth = playerManager.GetPlayerHealth();

            int newHealth = Mathf.Min(currentHealth + healAmount, playerManager.GetMaxHealth()); // prevent overhealing

            playerManager.SetPlayerHealth(newHealth);
        }
    }

    public bool GetIsBlocking() { return isBlocking; }

    public bool GetIsDead() { return isDead; }

    public void SetIsBlocking(bool newIsBlocking)
    {
        isBlocking = newIsBlocking;
    }

    public void StartInv() { gameObject.layer = invLayermask; }

    public void StopInv() { gameObject.layer = playerlayerMask; }


    private void Death()
    {
        isDead = true;
        
        animator.SetTrigger("IsDead");
        gameObject.GetComponent<PlayerMovement>().enabled = false; // prevent movement after death
        playerInput.SwitchCurrentActionMap("DisableMap");
        
        StartInv();

        playerManager.OnPlayerDeath();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("KillZone"))
        {
            playerManager.AdjustPlayerHealth(-1000000000);
        }
    }
}
