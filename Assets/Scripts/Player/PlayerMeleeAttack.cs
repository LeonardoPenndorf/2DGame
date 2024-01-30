using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerMeleeAttack : MonoBehaviour
{
    // public variables
    public int baseDamage;
    public float knockbackForce, maxCooldown, keyHoldDurationThreshold;
    public GameObject HurtBox; // melee attack hurt box

    // private variables
    private BoxCollider2D HurtBoxCollider;
    private Animator PlayerAnimator;
    private int damage;
    private float cooldown, keyHoldDuration; // after attacking cooldown must end before attacking again
    private bool isKeyHoldCoroutineRunning = false; // necessary to prevent the running of multiple instances of the same coroutine

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        HurtBoxCollider = HurtBox.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Return) && (cooldown <= 0) && !isKeyHoldCoroutineRunning) // registers when return has been pressed 
        {
            StartCoroutine(MeasureKeyHoldDuration()); // measures how long return is being pressed
        }

        if (Input.GetKeyUp(KeyCode.Return) && (cooldown <= 0)) // registers when return is no longer pressed
        {
            CheckAttack(); // check if regualr melee attack or special
        }
    }

    private void CheckAttack()
    {
        cooldown = maxCooldown;

        if (keyHoldDuration < keyHoldDurationThreshold) // if key was pressed, execute regular attack
        {
            MeleeAttack();
        }
        else // if key was held, execute special attack
        {
            SpecialAttack();
        }
    }

    private void MeleeAttack()
    {
        damage = baseDamage; // set damage to base damage
        PlayerAnimator.SetTrigger("MeleeAttack");
    }

    private void SpecialAttack()
    {
        damage = baseDamage * 2; // special attacks deal double damage
        PlayerAnimator.SetTrigger("SpecialAttack");
    }

    public void enableHurtboxCollider() // enable hurtbox collider at beginning of animation
    {
        HurtBoxCollider.enabled = true;
    }

    public void disableHurtboxCollider() // disable hurtbox collider at end of animation
    {
        HurtBoxCollider.enabled = false;
    }

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

    public int GetDamage() {  return damage; }
}
