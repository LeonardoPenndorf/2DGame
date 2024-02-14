using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int damage;

    // private variables
    private BoxCollider2D HurtBox;

    // Start is called before the first frame update
    void Start()
    {
        HurtBox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, transform);
    }

    private void enableHurtboxCollider() // enable hurtbox collider at beginning of animation
    {
        HurtBox.enabled = true;
    }

    private void disableHurtboxCollider() // disable hurtbox collider at end of animation
    {
        HurtBox.enabled = false;
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
