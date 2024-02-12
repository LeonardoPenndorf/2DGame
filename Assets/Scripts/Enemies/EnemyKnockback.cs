using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    // private variables
    private EnemyHealth enemyHealth;
    private Rigidbody2D EnemyRB;
    private Animator EnemyAnimator;
    private Transform PlayerTransform;
    private bool isStunned, facingRight;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        EnemyRB = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();

        PlayerTransform = GameObject.FindWithTag("Player").transform;
    }

    public void Knockback(Vector2 knockbackVector, float stunDuration)
    {
        if (enemyHealth.GetIsDead()) return; // cannot knockback corpses

        EnemyAnimator.SetBool("IsMoving", false);

        isStunned = true; // prevent player from moving
        facingRight = transform.localScale.x > 0;

        if (PlayerTransform.position.x < transform.position.x)
        {
            EnemyRB.velocity = facingRight ? knockbackVector : new Vector2(-knockbackVector.x, knockbackVector.y);
        }
        else
        {
            EnemyRB.velocity = facingRight ? new Vector2(-knockbackVector.x, knockbackVector.y) : knockbackVector;
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
