using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] float maxIFrames;

    // private variables
    private PlayerManager playerManager; // player manager stores persistent values such as health
    private PlayerBlock playerBlock;
    private Animator animator;
    private PlayerInput playerInput; // upon death switch input mapto disable input
    private float iFrames;
    private bool isDead = false, 
                 isBlocking = false; // when blocking takes less damage

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance; // there is only ever a single player manager instance
        playerBlock = GetComponent<PlayerBlock>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIFrames();

        CheckDeathCondition();
    }

    private void UpdateIFrames()
    {
        if (iFrames > 0) iFrames -= Time.deltaTime;
    }

    private void CheckDeathCondition()
    {
        if (playerManager.GetPlayerHealth() <= 0 && !isDead) Death();
    }

    public void TakeDamage(int damage, Transform hurtboxTransform)
    {
        if ((iFrames > 0) || isDead || (isBlocking && playerBlock.IsAttackComingFromFront(hurtboxTransform))) { return; }

        playerManager.AdjustPlayerHealth(-damage);

        if (playerManager.GetPlayerHealth() > 0)
        {
            animator.SetTrigger("IsHit");
        }

        iFrames = maxIFrames;
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

    private void Death()
    {
        isDead = true;
        animator.SetTrigger("IsDead");
        gameObject.GetComponent<PlayerMovement>().enabled = false; // prevent movement after death
        playerInput.SwitchCurrentActionMap("DisableMap");

        playerManager.OnPlayerDeath();
    }
}
