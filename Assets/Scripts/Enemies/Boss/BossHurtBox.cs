using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHurtBox : MonoBehaviour
{
    // private variables
    private BossHealth bossHealth;
    private PlayerHealth playerHealth;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        bossHealth = GetComponentInParent<BossHealth>();
        damage = GetComponentInParent<BossAttack>().damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<PlayerHealth>();

            playerHealth.TakeDamage(damage, transform);
        }
    }
}
