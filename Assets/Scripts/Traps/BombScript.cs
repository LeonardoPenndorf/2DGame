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

    // Start is called before the first frame update
    void Start()
    {
        TriggerCollider = GetComponent<CircleCollider2D>();
        BombAnimator = GetComponent<Animator>();
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
        Collider2D PlayerCollider = Physics2D.OverlapCircle(transform.position, ExplosionRadius, LayerMask.GetMask("Player"));

        if (PlayerCollider)
        {
            PlayerCollider.GetComponent<PlayerHealth>().TakeDamage(damage, transform);
            PlayerCollider.GetComponent<PlayerMovement>().KnockBack(KnockbackVector, stunDuration, transform);

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
