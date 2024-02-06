using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    // private variables
    private int damage;
    private Vector2 knockbackVector; // all attributes come from PlayerMeleeAttack a script component of the parent

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damage = GetComponentInParent<PlayerMeleeAttack>().GetDamage();
        knockbackVector = GetComponentInParent<PlayerMeleeAttack>().knockbackVector;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            collision.GetComponent<EnemyKnockback>().Knockback(knockbackVector, 5);
        }
    }
}
