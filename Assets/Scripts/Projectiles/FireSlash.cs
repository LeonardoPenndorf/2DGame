using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlash : MonoBehaviour
{
    // public variables
    public int damage;

    // private variables
    private CircleCollider2D FireSlashCollider;
    private bool groundCollision, enemyCollision;

    // Start is called before the first frame update
    void Start()
    {
        FireSlashCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyCollision = FireSlashCollider.IsTouchingLayers(LayerMask.GetMask("Enemies"));
        groundCollision = FireSlashCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (enemyCollision)
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if (groundCollision)
        {
            Destroy(gameObject);
        }
    }
}
