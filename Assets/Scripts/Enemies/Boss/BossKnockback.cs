using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnockback : MonoBehaviour
{
    private BossHealth bossHealth;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform aimTransform;
    private bool isStunned, facingRight;

    // Start is called before the first frame update
    void Start()
    {
        bossHealth = GetComponent<BossHealth>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        aimTransform = GameObject.FindWithTag("Player").transform.Find("AimPoint").transform;
    }

    public void Knockback(Vector2 knockbackVector, float stunDuration)
    {
        if (bossHealth.GetIsDead()) return;

        animator.SetBool("IsMoving", false);

        isStunned = true; // prevent player from moving
        facingRight = transform.localScale.x > 0;

        if (aimTransform.position.x < transform.position.x)
        {
            rb.velocity = facingRight ? knockbackVector/2 : new Vector2(-knockbackVector.x, knockbackVector.y);
        }
        else
        {
            rb.velocity = facingRight ? new Vector2(-knockbackVector.x, knockbackVector.y) : knockbackVector;
        }

        StartCoroutine(Stunned(stunDuration));
    }

    IEnumerator Stunned(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    public bool GetIsStunned() { return isStunned; }
}
