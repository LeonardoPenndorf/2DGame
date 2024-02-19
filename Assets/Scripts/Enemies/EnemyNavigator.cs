using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNavigator : MonoBehaviour
{
    [SerializeField] float wait = 1;

    // private variables
    private Collider2D navCollider;
    private EnemyMovement enemyMovement;
    private EnemyAggro enemyAggro;
    private int groundLayerMask, platformsLayerMask;
    private bool canRotate = false, enemyInFront = false;

    // Start is called before the first frame update
    void Start()
    {
        navCollider = GetComponent<Collider2D>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemyAggro = GetComponentInParent<EnemyAggro>();

        groundLayerMask = LayerMask.GetMask("Ground");
        platformsLayerMask = LayerMask.GetMask("Platforms");

        StartCoroutine(Wait());
    }

    public bool GetEnemyInFront() {  return enemyInFront; }

    public bool CheckNav()
    {
        return navCollider.IsTouchingLayers(groundLayerMask) || navCollider.IsTouchingLayers(platformsLayerMask);
    }

    public void CheckDirection()
    {
        if (!CheckNav() && !enemyAggro.GetIsAggroed())
            enemyMovement.SetDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && enemyAggro.GetIsAggroed())
            CheckEnemy(collision);

        if (!canRotate) return;

        if (collision.CompareTag("Trap") && !enemyAggro.GetIsAggroed())
            enemyMovement.SetDirection();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && enemyAggro.GetIsAggroed())
        {
            enemyInFront = false;
        }

        if (!canRotate) return;

        if (!CheckNav() && !enemyAggro.GetIsAggroed())
            enemyMovement.SetDirection();
    }


    private void CheckEnemy(Collider2D collision) // check if a melee enemy is in front to prevent overlap
    {
        if (collision.GetComponent<EnemyHealth>().GetIsDead())
        {
            enemyInFront = false;
            return;
        }

        if (collision.GetComponent<EnemyRangedAttack>() == null)
        {
            enemyInFront = true;
            return;
        }

        enemyInFront = false;
    }

    private IEnumerator Wait() // wait a bit before being allowed to rotate
    {
        yield return new WaitForSeconds(wait);
        canRotate = true;
    }
}
