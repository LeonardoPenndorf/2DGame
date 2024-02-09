using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // public variables
    public int damage;

    // private variables
    private BoxCollider2D ArrowCollider;
    private bool groundCollision, playerCollision, enemyCollision;

    // Start is called before the first frame update
    void Start()
    {
        ArrowCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerCollision = ArrowCollider.IsTouchingLayers(LayerMask.GetMask("Player"));
        enemyCollision = ArrowCollider.IsTouchingLayers(LayerMask.GetMask("Enemies"));
        groundCollision = ArrowCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if(playerCollision)
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, transform);
            Destroy(gameObject);
        }
        else if (enemyCollision) // has friendly fire
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (groundCollision)
        {
            Destroy(gameObject);
        }
    }
}
