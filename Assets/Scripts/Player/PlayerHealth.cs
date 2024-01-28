using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // public variables
    public int maxHealth, currentHealth;

    // private variables
    private Animator PlayerAnimator;
    private BoxCollider2D PlayerCollider;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerCollider = GetComponent<BoxCollider2D>();

        currentHealth = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // chek for enemy collision
        {
            int damage = collision.gameObject.GetComponent<EnemyStats>().damage; // get enemy damage
            TakeDamge(damage);
        }
        else if (collision.gameObject.CompareTag("Healing"))
        {
            Heal();
        }
    }

    private void TakeDamge(int damage)
    {
        PlayerAnimator.SetTrigger("IsHit"); // trigger hit animation
        currentHealth -= damage;
    }

    private void Heal()
    {
        Debug.Log("Placeholder");
    }
}
