using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    // private variables
    private Rigidbody2D EnemyRB;
    private Animator EnemyAnimator;
    private bool isStunned;

    // Start is called before the first frame update
    void Start()
    {
        EnemyRB = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
    }

    public void Knockback(Vector2 knockbackVector, float stunDuration)
    {
        EnemyAnimator.SetBool("IsMoving", false);

        isStunned = true; // prevent player from moving

        EnemyRB.velocity = knockbackVector;

        StartCoroutine(Stunned(stunDuration));
    }

    IEnumerator Stunned(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
    }

    public bool GetIsStunned() { return isStunned; }
}
