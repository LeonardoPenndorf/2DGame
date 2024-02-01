using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // public variables
    public float standardMovementSpeed, aggroedMovementSpeed, aggroRange;
    public BoxCollider2D NavCollider; // checks for collison with ground and player
    public string[] animationsArray; // array containing the name of all animations that would stop the enemy from moving

    // private varibales
    private Rigidbody2D EnemyRigidbody;
    private Animator EnemyAnimator;
    private AnimationChecker animationsChecker; // class containing functions to check which animtions are running
    private GameObject Player; // player gameobject is required for navigation when aggroed
    private float movementSpeed, attackRange, direction = 1.0f;
    private bool isAggroed, checkNav;
    private float distance, xDistance; // distance between the enemy and the player

    // Start is called before the first frame update
    void Start()
    {
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
        animationsChecker = GetComponent<AnimationChecker>();

        Player = GameObject.FindWithTag("Player");

        movementSpeed = standardMovementSpeed; // set movement speed

        attackRange = gameObject.GetComponent<EnemyMeleeAttack>().attackRange; // fetch attack range to determine how close the enemy should get to the player to attack
    }

    // Update is called once per frame
    void Update()
    {
        if (isAggroed)
        {
            MoveTowardsPlayer(); // if aggroed guides enemy movement
        }
        else
        {
            CheckAggro(); // if not, check if player is within aggro range
        }

        if (!animationsChecker.CheckAnimations(animationsArray)) // cannot move during certain animations
        {
            Move(); // moves and rotates the enemy
        }
        else
        {
            EnemyRigidbody.velocity = new Vector2(0, 0);
        }

        EnemyAnimator.SetBool("IsMoving", movementSpeed != 0);
    }

    private void Move()
    {
        EnemyRigidbody.velocity = new Vector2(direction * movementSpeed, EnemyRigidbody.velocity.y);
        
        if (direction > 0) // turn left
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (direction < 0) // turn right
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void MoveTowardsPlayer()
    {
        checkNav = NavCollider.IsTouchingLayers(LayerMask.GetMask("Ground")); // check if can walk
        distance = Vector2.Distance(Player.transform.position, transform.position);
        xDistance = Mathf.Abs(Player.transform.position.x - transform.position.x);

        if (!checkNav || (distance < attackRange) || (xDistance < attackRange)) // either cannot walk or player within attack range
        {
            movementSpeed = 0;
        }
        else
        {
            movementSpeed = aggroedMovementSpeed; // when aggroed moves faster
        }

        // checks if player is to the right or to the left
        if (Player.transform.position.x < transform.position.x)
        {
            // Player is to the left
            direction = -1.0f;
        }
        else if (Player.transform.position.x > transform.position.x)
        {
            // Player is to the right
            direction = 1.0f;
        }
    }

    private void CheckAggro() // check if player is within aggro range
    {
        Collider2D PlayerCollider = Physics2D.OverlapCircle(transform.position, aggroRange, LayerMask.GetMask("Player"));

        if (PlayerCollider)
        {
            isAggroed = true;
            EnemyAnimator.SetBool("IsAggroed", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        checkNav = NavCollider.IsTouchingLayers(LayerMask.GetMask("Ground")); // check if can walk

        if (!checkNav && !isAggroed)
            direction = -direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
