using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    // public variables
    public GameObject HurtBox; // melee attack hurt box
    public int baseDamage; // base damage of all attacks. This vlaue is used to set the damage of a specifc attack
    public float maxComboDelay, // maximum amount of time between clicks
                 knockbackForce, // the knockback force of an attack (not yet implemented)
                 AirAttackCooldown, // after air attacking cooldown must end before attacking again 
                 keyHoldDurationThreshold; // amount of time that needs to pass to register as hoplding down

    // private variables
    private BoxCollider2D HurtBoxCollider;
    private Animator PlayerAnimator;
    private PlayerMovement PlayerMovementComponent;
    private int damage, // damage of the current attack
                attackComboCounter = 0; // records the cuurent combo account, this values ranges between 0 and 3
    private float lastClickTime, // the last time the player has clicked the attack button
                  cooldownTimer,
                  keyHoldDuration; // measures how long they key is being held to differentiate between holding and clicking
    private bool isGrounded, isKeyHoldCoroutineRunning = false; // necessary to prevent the running of multiple instances of the same coroutine

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
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Return) && !isKeyHoldCoroutineRunning) // registers when return has been pressed 
        {
            StartCoroutine(MeasureKeyHoldDuration()); // measures how long return is being pressed
        }

        if (Input.GetKeyUp(KeyCode.Return)) // registers when return is no longer pressed
        {
            CheckAttack(); // check if regualr melee attack or special
        }
    }

    private void CheckAttack() // check which attack should be executed
    {
        isGrounded = PlayerMovementComponent.GetIsGrounded();

        if (keyHoldDuration < keyHoldDurationThreshold && isGrounded) // if key was pressed and is grounded, execute regular attack
        {
            AttackCombo();
        }
        else if (keyHoldDuration < keyHoldDurationThreshold && !isGrounded && cooldownTimer <= 0) // if key was pressed and is not grounded, execute air attack
        {
            AirAttack();
        }
        else if (keyHoldDuration >= keyHoldDurationThreshold && isGrounded) // if key was held and is grounded, execute special attack
        {
            SpecialAttack();
        }
    }

    private void AttackCombo() // a 3 hit attack combo
    {
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

        cooldownTimer = AirAttackCooldown;
    }

    private void SpecialAttack() // a very powerful attack that also shoots a projectile
    {
        damage = baseDamage * 4; // special attacks deals a lot of damage
        PlayerAnimator.SetTrigger("SpecialAttack");
    }

    public void enableHurtboxCollider() // enable hurtbox collider at beginning of attack animation
    {
        HurtBoxCollider.enabled = true;
    }

    public void disableHurtboxCollider() // disable hurtbox collider at end of attack animation
    {
        HurtBoxCollider.enabled = false;
    }

    public void ResetCounter() // called in the attack end animations and the third attack
    {
        PlayerAnimator.SetInteger("AttackComboCounter", 0);
    }
    public int GetDamage() { return damage; }

    private IEnumerator MeasureKeyHoldDuration()
    {
        isKeyHoldCoroutineRunning = true;
        keyHoldDuration = 0.0f;

        while (Input.GetKey(KeyCode.Return))
        {
            keyHoldDuration += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        isKeyHoldCoroutineRunning= false;
    }

    private IEnumerator ResetComboAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        attackComboCounter = 0; // Reset the combo counter after the delay
    }
}
