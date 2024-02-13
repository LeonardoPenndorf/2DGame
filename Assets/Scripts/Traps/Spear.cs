using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int damage;

    // private variables
    private BoxCollider2D spearCollider;

    // Start is called before the first frame update
    void Start()
    {
        spearCollider = GetComponent<BoxCollider2D>();
    }

    private void EnableHurtbox()
    {
        spearCollider.enabled = true;
    }

    private void disableHurtbox()
    {
        spearCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, transform);

        if (collision.CompareTag("Enemy"))
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
    }
}
