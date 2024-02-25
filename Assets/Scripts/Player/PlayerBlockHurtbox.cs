using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockHurtbox : MonoBehaviour
{
    // private variables
    private GameObject enemy;
    private float stunDuration;
    private Vector2 modifierVector = new Vector2(0.5f, 0.5f),
                    knockbackVector; // all attributes come from PlayerMeleeAttack a script component of the parent

    // Start is called before the first frame update
    void Start()
    {
        stunDuration = GetComponentInParent<PlayerMeleeAttack>().GetStunDuration();
        knockbackVector = GetComponentInParent<PlayerMeleeAttack>().GetKnockbackVector() * modifierVector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Hurtbox") || collision.transform.parent == null || !collision.transform.parent.CompareTag("Enemy")) return;

        enemy = collision.transform.parent.gameObject;
        enemy.GetComponent<Animator>().SetTrigger("IsHit");
        enemy.GetComponent<EnemyKnockback>().Knockback(knockbackVector, stunDuration);
    }
}
