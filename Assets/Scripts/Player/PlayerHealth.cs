using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // public variables
    public int maxHealth, currentHealth;
    public float maxInvTime;

    // private variables
    private Animator PlayerAnimator;
    private float invTime;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (invTime > 0)
        {
            invTime -= Time.deltaTime;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy") && (invTime <= 0)) // chek for enemy collision and if invincibilty time is over
    //    {
    //        int damage = collision.gameObject.GetComponent<EnemyStats>().damage; // get enemy damage
    //        TakeDamge(damage);
    //    }
    //}

    public void TakeDamge(int damage)
    {
        PlayerAnimator.SetTrigger("IsHit"); // trigger hit animation
        currentHealth -= damage;
        invTime = maxInvTime;
    }
}
