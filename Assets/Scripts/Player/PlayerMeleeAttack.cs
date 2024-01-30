using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerMeleeAttack : MonoBehaviour
{
    // public variables
    public int damage;
    public float knockbackForce, maxCooldown;
    public GameObject HurtBox; // melee attack hurt box

    // private variables
    private Animator PlayerAnimator;
    private float cooldown; // after attacking cooldown must end before attacking again

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        MeleeAttack();
    }

    private void MeleeAttack()
    {
        if ((Input.GetKeyDown(KeyCode.Return)) && (cooldown <= 0))
        {
            PlayerAnimator.SetTrigger("MeleeAttack");
            cooldown = maxCooldown;
        }
    }

    public void enableHurtboxCollider() // enable hurtbox collider at beginning of animation
    {
        HurtBox.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void disableHurtboxCollider() // disable hurtbox collider at end of animation
    {
        HurtBox.GetComponent<BoxCollider2D>().enabled = false;
    }
}
