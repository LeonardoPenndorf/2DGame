using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] float viewDistance,
                           aggroRange,
                           fovAngle, 
                           looseAggroTime = 5f; // Time in seconds to loose aggro

    // private variables
    private Animator EnemyAnimator;
    private Transform Player;
    private float timeSinceLastSeen;
    private int playerLayerMask;
    private bool playerDetected = false, 
                 isAggroed = false;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        playerLayerMask = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerDetected = DetectPlayerWithFOV() || DetectPlayerWithRadius();

        if (!isAggroed)
        {
            if(playerDetected)
            {
                isAggroed = true;
                EnemyAnimator.SetBool("IsAggroed", true);
                timeSinceLastSeen = 0f; // Reset timer
            }
        }
        else
        {
            if (playerDetected)
            {
                timeSinceLastSeen = 0f; // Reset timer since player is still within aggro range/FOV
            }
            else
            {
                timeSinceLastSeen += Time.deltaTime;
                if (timeSinceLastSeen >= looseAggroTime)
                {
                    LooseAggro();
                }
            }
        }
    }

    private bool DetectPlayerWithFOV() // detect enemy with field of view field of view
    {
        Vector2 directionToPlayer = Player.position - transform.position;
        float angleBetweenEnemyAndPlayer = Vector2.Angle(-transform.right, directionToPlayer);

        if (directionToPlayer.magnitude < viewDistance && angleBetweenEnemyAndPlayer < fovAngle / 2)
            return true;
        
        return false;
    }

    private bool DetectPlayerWithRadius()  // detect player with radius around enemy
    {
        return Physics2D.OverlapCircle(transform.position, aggroRange, playerLayerMask);
    }

    private void LooseAggro()
    {
        isAggroed = false;
        EnemyAnimator.SetBool("IsAggroed", false);
        timeSinceLastSeen = 0f; // Reset timer as a precaution
    }

    public bool GetIsAggroed() { return isAggroed; }

    void OnDrawGizmos()
    {
        if (Player == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

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
