using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float stunDuration, timer;
    [SerializeField] int damage;
    [SerializeField] Vector2 KnockbackVector;

    private Rigidbody2D rb;
    private Animator animator;
    private HomingMissile homingMissile;
    private CircleCollider2D circleCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        homingMissile = GetComponent<HomingMissile>();
        circleCollider = GetComponent<CircleCollider2D>();

        StartCoroutine(LifeTime());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, transform);
            collision.GetComponent<PlayerMovement>().KnockBack(KnockbackVector, stunDuration, transform);
            StartExplosion();
            return;
        }

        if (circleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            StartExplosion();
        }
    }

    private void StartExplosion()
    {
        animator.SetTrigger("Explosion");
        homingMissile.enabled = false;
        rb.velocity = Vector3.zero;
        circleCollider.enabled = false;
    }

    private void SelfDestruct() { Destroy(gameObject); } // called at the end of the explosion animation

    private IEnumerator LifeTime() // explode after timer runs out
    {
        yield return new WaitForSeconds(timer);
        StartExplosion();
    }
}
