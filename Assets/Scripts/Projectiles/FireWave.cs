using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWave : MonoBehaviour
{
    [SerializeField] float timer;

    // private variables
    private PlayerManager playerManager; // player manager stores persistent values such as health
    private Collider2D fireWaveCollider;
    private EnemyHealth enemyHealth;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        fireWaveCollider = GetComponent<Collider2D>();
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

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
