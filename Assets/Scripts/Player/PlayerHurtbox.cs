using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    // private variables
    private int damage;
    private float knockbackForce; // all attributes come from PlayerMeleeAttack a script component of the parent

    // Start is called before the first frame update
    void Start()
    {
        damage = GetComponentInParent<PlayerMeleeAttack>().damage;
        knockbackForce = GetComponentInParent<PlayerMeleeAttack>().knockbackForce;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Health>().TakeDamge(damage);
        }
    }
}
