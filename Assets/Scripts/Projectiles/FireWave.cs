using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWave : MonoBehaviour
{
    [SerializeField] float timer;

    // private variables
    private PlayerManager playerManager; // player manager stores persistent values such as health
    private Collider2D fireWaveCollider, 
                       navCollider;
    private EnemyHealth enemyHealth;
    private int damage, 
                groundLayerMask, 
                platformsLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        fireWaveCollider = GetComponent<Collider2D>();
        navCollider = transform.Find("Navigator").GetComponent<Collider2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
        platformsLayerMask = LayerMask.GetMask("Platforms");
        playerManager = PlayerManager.instance;

        damage = playerManager.GetDamage() / 2;

        StartCoroutine(LifeTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (fireWaveCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            enemyHealth = collision.GetComponent<EnemyHealth>();

            if (enemyHealth != null && !enemyHealth.GetIsDead())
                collision.GetComponent<EnemyHealth>().TakeDamage(damage, transform);
        }

        if (fireWaveCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            Destroy(gameObject);
    }

    private void CheckGround() // called in the firewave animation
    {
        if (!navCollider.IsTouchingLayers(groundLayerMask) && !navCollider.IsTouchingLayers(platformsLayerMask))
            Destroy(gameObject);
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
