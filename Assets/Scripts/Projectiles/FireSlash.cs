using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlash : MonoBehaviour
{
    // public variables
    public int damage;

    // private variables
    private CircleCollider2D FireSlashCollider;
    private EnemyHealth enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        FireSlashCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (FireSlashCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null && !enemyHealth.GetIsDead())
            {
                collision.GetComponent<EnemyHealth>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        if (FireSlashCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            Destroy(gameObject);
        }
    }
}
