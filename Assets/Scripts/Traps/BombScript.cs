using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] float ExplosionRadius, stunDuration;
    [SerializeField] int damage;
    [SerializeField] Vector2 KnockbackVector;

    // private variables
    private CircleCollider2D TriggerCollider;
    private Animator BombAnimator;
    private bool playerNearby;
    private int playerLayerMask, enemiesLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        TriggerCollider = GetComponent<CircleCollider2D>();
        BombAnimator = GetComponent<Animator>();
        
        playerLayerMask = LayerMask.GetMask("Player");
        enemiesLayerMask = LayerMask.GetMask("Enemies");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerNearby = TriggerCollider.IsTouchingLayers(LayerMask.GetMask("Player"));

        if (playerNearby)
        {
            BombAnimator.SetTrigger("PlayerNearby");
        }
    }

    private void Explosion() // collider is activated when explosion animation starts
    {
        Collider2D PlayerCollider = Physics2D.OverlapCircle(transform.position, ExplosionRadius, playerLayerMask);

        if (PlayerCollider)
        {
            PlayerCollider.GetComponent<PlayerHealth>().TakeDamage(damage, transform);
            PlayerCollider.GetComponent<PlayerMovement>().KnockBack(KnockbackVector, stunDuration, transform);

        }

        Collider2D[] enemyCollision = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius, enemiesLayerMask);

        foreach (Collider2D collision in enemyCollision)
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            collision.GetComponent<EnemyKnockback>().Knockback(KnockbackVector, stunDuration);
        }
    }

    private void SelfDestruct() // remove bomb after expolsion
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
