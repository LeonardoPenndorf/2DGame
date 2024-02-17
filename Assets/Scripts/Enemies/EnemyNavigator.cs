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
    private bool canRotate = false;

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
        if (!canRotate) return;

        if (collision.CompareTag("Trap") && !enemyAggro.GetIsAggroed())
            enemyMovement.SetDirection();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!canRotate) return;

        if (!CheckNav() && !enemyAggro.GetIsAggroed())
            enemyMovement.SetDirection();
    }

    private IEnumerator Wait() // wait a bit before being allowed to rotate
    {
        yield return new WaitForSeconds(wait);
        canRotate = true;
    }
}
