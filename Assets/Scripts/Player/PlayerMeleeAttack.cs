using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttack : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] GameObject HurtBox; // melee attack hurt box
    [SerializeField] float maxComboDelay, // maximum amount of time between clicks
                           attackCooldown, // after air attacking cooldown must end before attacking again 
                           stunDuration; // after being hit by an attack, the enemy is stunned for a short while
    [SerializeField] int baseDamage; // base damage of all attacks. This vlaue is used to set the damage of a specifc attack
    [SerializeField] Vector2 knockbackVector; // the knockback force of an attack

    // private variables
    private BoxCollider2D HurtBoxCollider;
    private Animator PlayerAnimator;
    private PlayerMovement PlayerMovementComponent;
    private int damage, // damage of the current attack
                attackComboCounter = 0; // records the cuurent combo account, this values ranges between 0 and 3
    private float lastClickTime, // the last time the player has clicked the attack button
                  cooldownTimer;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        HurtBoxCollider = HurtBox.GetComponent<BoxCollider2D>();
        PlayerMovementComponent = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TogglePauseMenu.gameIsPaused) return;

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void CheckAttack(InputAction.CallbackContext context) // check which attack should be executed
    {
        if (TogglePauseMenu.gameIsPaused || cooldownTimer > 0) return;

        if (!context.performed) return; // only perform this function once

        isGrounded = PlayerMovementComponent.GetIsGrounded();

        if (isGrounded)
        {
            AttackCombo();
        }
        else
        {
            AirAttack();
        }
    }

    private void AttackCombo() // a 3 hit attack combo
    {
        PlayerAnimator.SetInteger("AttackComboCounter", 1);

        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick <= maxComboDelay)
        {
            attackComboCounter++;
        }
        else
        {
            attackComboCounter = 1; // Reset counter if too much time has passed
        }

        lastClickTime = Time.time; // Update the last click time

        attackComboCounter = Mathf.Clamp(attackComboCounter, 1, 3); // Clamp the click counter to ensure it's within combo range

        damage = baseDamage; // all combo attack except the last one deal base damage

        switch (attackComboCounter)
        {
            case 1:
                PlayerAnimator.SetInteger("AttackComboCounter", 1);
                break;
            case 2:
                PlayerAnimator.SetInteger("AttackComboCounter", 2);
                break;
            case 3:
                damage = baseDamage * 2; // Max combo damage
                PlayerAnimator.SetInteger("AttackComboCounter", 3);
                cooldownTimer = attackCooldown;
                break;
        }

        // Reset combo counter if max combo is reached or too much time passes between attacks
        if (attackComboCounter == 3 || timeSinceLastClick > maxComboDelay)
        {
            StartCoroutine(ResetComboAfterDelay(maxComboDelay));
        }
    }

    private void AirAttack() // a single attack that can  be performed mid air
    {
        damage = baseDamage; // set damage to base damage
        PlayerAnimator.SetTrigger("AirAttack");

        cooldownTimer = attackCooldown;
    }

    public void SpecialAttack(InputAction.CallbackContext context) // a very powerful attack that also shoots a projectile
    {
        if (TogglePauseMenu.gameIsPaused || cooldownTimer > 0) return;

        if (context.performed)
        {
            damage = baseDamage * 4; // special attacks deals a lot of damage
            PlayerAnimator.SetTrigger("SpecialAttack");
            cooldownTimer = attackCooldown;
        }
    }

    private void enableHurtboxCollider() // enable hurtbox collider at beginning of attack animation
    {
        HurtBoxCollider.enabled = true;
    }

    private void disableHurtboxCollider() // disable hurtbox collider at end of attack animation
    {
        HurtBoxCollider.enabled = false;
    }

    private void ResetCounter() // called in the attack end animations and the third attack
    {
        PlayerAnimator.SetInteger("AttackComboCounter", 0);
    }

    public int GetDamage() { return damage; }

    public float GetStunDuration() { return stunDuration; }

    public Vector2 GetKnockbackVector() { return knockbackVector; }

    private IEnumerator ResetComboAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        attackComboCounter = 0; // Reset the combo counter after the delay
    }
}
