using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    // private variables
    private int damage; 

    // Start is called before the first frame update
    void Start()
    {
        damage = GetComponentInParent<EnemyMeleeAttack>().damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, transform);

        }
    }
}
