using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int maxHealth, lifeSteal;

    // private variables
    private Animator animator;
    private int currentHealth;

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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("IsHit");
    }

    public void Heal() { currentHealth += lifeSteal; }
}
