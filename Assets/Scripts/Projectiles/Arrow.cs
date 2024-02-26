using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int damage;

    // private variables
    private Collider2D ArrowCollider;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        ArrowCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, transform);
            Destroy(gameObject);
        }
        else if (ArrowCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) // when arrows collide with the wall they get stuck and fade away after a short delay
        {
            rb.velocity = Vector3.zero;
            ArrowCollider.enabled = false;
        }
    }
}
