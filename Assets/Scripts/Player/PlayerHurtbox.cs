using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    // private variables
    private int damage;
    private float knockbackForce; // all attributes come from PlayerMeleeAttack a script component of the parent

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damage = GetComponentInParent<PlayerMeleeAttack>().GetDamage();
        knockbackForce = GetComponentInParent<PlayerMeleeAttack>().knockbackForce;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealth>().TakeDamge(damage);
        }
    }
}
