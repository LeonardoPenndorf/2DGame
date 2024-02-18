using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallDamage : MonoBehaviour
{
    [SerializeField] float fallDamageThreshold;
    [SerializeField] int damage;

    private Rigidbody2D rb;
    private PlayerHealth health;
    private float peakFallSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<PlayerHealth>();
    }
    void Update()
    {
        if (rb.velocity.y < 0) // Check if the player is falling
            peakFallSpeed = Mathf.Min(peakFallSpeed, rb.velocity.y); // Track the peak (most negative) fall speed
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;

        if (peakFallSpeed < fallDamageThreshold)
        {
            health.TakeDamage(damage, transform);
        }
        peakFallSpeed = 0;
    }
}
