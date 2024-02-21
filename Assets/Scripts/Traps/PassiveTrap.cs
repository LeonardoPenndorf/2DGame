using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveTrap : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int damage;

    // private variables
    private Collider2D trapCollider;

    // Start is called before the first frame update
    void Start()
    {
        trapCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, transform);

        if (collision.CompareTag("Enemy"))
            collision.GetComponent<EnemyHealth>().TakeDamage(damage, transform);
    }
}
