using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyRevive : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] float reviveRadius, maxCooldown;

    // private variables
    private float cooldown;
    private int enemyLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        enemyLayerMask = LayerMask.GetMask("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        if (cooldown <= 0)
        {
            ReviveEnemies();
        }
    }

    private void ReviveEnemies()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, reviveRadius, enemyLayerMask);

        foreach (Collider2D collider in hitColliders)
        {
            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Revive();
            }
        }

        cooldown = maxCooldown;
    }
}
