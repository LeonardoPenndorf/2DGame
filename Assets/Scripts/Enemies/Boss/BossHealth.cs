using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int maxHealth;

    // private variables
    private Animator animator;
    private int currentHealth;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        DisplayHealthbar();
    }

    private void DisplayHealthbar()
    {
        Canvas canvas = GameObject.FindWithTag("BossHealthbar").GetComponent<Canvas>();
        canvas.enabled = true;
    }
    public int GetMaxHealth() { return maxHealth; }

    public int GetCurrentHealth() { return currentHealth; }

    public bool GetIsDead() { return isDead; }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("IsHit");
    }
}
