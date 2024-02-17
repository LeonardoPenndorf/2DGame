using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearHurtBox : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, transform);

        if (collision.CompareTag("Enemy"))
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
    }
}
