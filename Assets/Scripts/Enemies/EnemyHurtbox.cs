using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    // private variables
    private int damage; 
    private float knockbackForce, stunDuration; // all attributes come from EnemyMeleeAttck a script component of the parent

    // Start is called before the first frame update
    void Start()
    {
        damage = GetComponentInParent<EnemyMeleeAttack>().damage;
        knockbackForce = GetComponentInParent<EnemyMeleeAttack>().knockbackForce;
        stunDuration = GetComponentInParent<EnemyMeleeAttack>().stunDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamge(damage);

        }
    }
}
