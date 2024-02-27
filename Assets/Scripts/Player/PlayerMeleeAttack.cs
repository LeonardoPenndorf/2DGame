using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttack : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] GameObject HurtBox; // melee attack hurt box
    [SerializeField] float holdThreshold,
                           maxComboDelay, // maximum amount of time between clicks
                           attackCooldown, // after air attacking cooldown must end before attacking again 
                           stunDuration; // after being hit by an attack, the enemy is stunned for a short while
    [SerializeField] Vector2 knockbackVector; // the knockback force of an attack

    // private variables
    private PlayerManager playerManager; // player manager stores persistent values such as health
    private BoxCollider2D HurtBoxCollider;
    private Animator PlayerAnimator;
    private AnimationChecker animationChecker;
    private TogglePauseGame togglePauseGame;
    private PlayerFeet playerFeet;
    private int damage; // damage of the current attack
    private float cooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        HurtBoxCollider = HurtBox.GetComponent<BoxCollider2D>();
        animationChecker = GetComponent<AnimationChecker>();
        playerManager = PlayerManager.instance;
        togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();
        playerFeet = transform.Find("PlayerFeet").GetComponent<PlayerFeet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (togglePauseGame.GetGameIsPaused()) return;

        cooldownTimer -= Time.deltaTime;
    }


    public void CheckAttack(InputAction.CallbackContext context) // check which attack should be executed
    {
        if (togglePauseGame.GetGameIsPaused() || cooldownTimer > 0 || !context.performed || context.duration > holdThreshold) return;

        if (playerFeet.GetIsGrounded())
        {
            AttackCombo();
        }
        else
        {
            AirAttack();
        }

        int comboCounter = PlayerAnimator.GetInteger("AttackComboCounter");
        if (comboCounter == 0 || comboCounter == 3)
        {
            cooldownTimer = attackCooldown;
        }
    }

    private void AttackCombo() // a 3 hit attack combo
    {
        switch (animationChecker.GetCurrentAnimation())
        {
            case "Warrior_MeleeAttack_3":
                PlayerAnimator.SetInteger("AttackComboCounter", 0);
                break;
            case "Warrior_MeleeAttack_1":
                PlayerAnimator.SetInteger("AttackComboCounter", 2);
                break;
            case "Warrior_MeleeAttack_2":
                PlayerAnimator.SetInteger("AttackComboCounter", 3);
                break;
            default:
                PlayerAnimator.SetInteger("AttackComboCounter", 1);
                break;
        }
    }

    private void AirAttack() // a single attack that can  be performed mid air
    {
        damage = playerManager.GetDamage() / 2;
        PlayerAnimator.SetTrigger("AirAttack");
    }

    public void SpecialAttack(InputAction.CallbackContext context) // a very powerful attack that also shoots a projectile
    {
        if (togglePauseGame.GetGameIsPaused() || cooldownTimer > 0 || !context.performed) return;

        damage = playerManager.GetDamage() * 2; // special attacks deals a lot of damage
        PlayerAnimator.SetTrigger("SpecialAttack");
        cooldownTimer = attackCooldown;
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

    private void SetFirstAtkDamage() { damage = playerManager.GetDamage(); }

    private void SetSecondAtkDamage() 
    {
        damage = playerManager.GetDamage();
        damage += (int)(damage * 0.25f);
    }

    private void SetThirdAtkDamage()
    {
        damage = playerManager.GetDamage(); 
        damage += (int)(damage * 0.5f);
    }

    public int GetDamage() { return damage; }

    public float GetStunDuration() { return stunDuration; }

    public Vector2 GetKnockbackVector() { return knockbackVector; }
}
