using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    // public variables
    public float viewDistance,
                 aggroradius,
                 fovAngle;

    // private variables
    private Animator EnemyAnimator;
    private Transform Player;
    private bool isAggroed = false;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAggroed)
        {
            DetectPlayerFOV();
            DetectPlayerRadius();
        }
    }

    private void DetectPlayerFOV()
    {
        Vector2 directionToPlayer = Player.position - transform.position;
        float angleBetweenEnemyAndPlayer = Vector2.Angle(-transform.right, directionToPlayer);

        if (directionToPlayer.magnitude < viewDistance && angleBetweenEnemyAndPlayer < fovAngle / 2)
        {
            isAggroed = true;
            EnemyAnimator.SetBool("IsAggroed", true);
        }
    }

    private void DetectPlayerRadius()
    {
    }

    private void LooseAggro()
    {
        Debug.Log("Placeholder");
    }

    public bool GetIsAggroed() { return isAggroed; }

    void OnDrawGizmos()
    {
        if (Player == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector2 fovLeftBoundary = Quaternion.Euler(0, 0, -fovAngle / 2) * (-transform.right) * viewDistance;
        Vector2 fovRightBoundary = Quaternion.Euler(0, 0, fovAngle / 2) * (-transform.right) * viewDistance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + fovLeftBoundary);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + fovRightBoundary);

        Vector2 directionToPlayer = Player.position - transform.position;
        float angleBetweenEnemyAndPlayer = Vector2.Angle(-transform.right, directionToPlayer);
        if (directionToPlayer.magnitude < viewDistance && angleBetweenEnemyAndPlayer < fovAngle / 2)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawLine(transform.position, Player.position);
    }
}
