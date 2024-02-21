using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    // private variables
    private int damage;
    private float stunDuration;
    private Vector2 knockbackVector; // all attributes come from PlayerMeleeAttack a script component of the parent

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damage = GetComponentInParent<PlayerMeleeAttack>().GetDamage();
        stunDuration = GetComponentInParent<PlayerMeleeAttack>().GetStunDuration();
        knockbackVector = GetComponentInParent<PlayerMeleeAttack>().GetKnockbackVector();

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage, transform);
            collision.GetComponent<EnemyKnockback>().Knockback(knockbackVector, stunDuration);
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.GetComponent<BossHealth>().TakeDamage(damage);
        }
    }
}
