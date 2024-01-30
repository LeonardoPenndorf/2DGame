using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerMeleeAttack : MonoBehaviour
{
    // public variables
    public int damage;
    public float knockbackForce, maxCooldown, hurtBoxRadius;
    public Transform HurtBox; // melee attack hurt box

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

            Collider2D[] EnemyColliderArray = Physics2D.OverlapCircleAll(HurtBox.transform.position, hurtBoxRadius, LayerMask.GetMask("Enemies")); // get the collider of all enemies iht by the attack

            foreach(Collider2D EnemyCollider in EnemyColliderArray)
            {
                EnemyCollider.GetComponent<Health>().TakeDamge(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(HurtBox.transform.position, hurtBoxRadius);
    }
}
