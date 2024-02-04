using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // public variables
    public float standardMovementSpeed, // movement speed of enemy when not aggroed
                 aggroedMovementSpeed; // some enemies move faster when aggroed
    public BoxCollider2D NavCollider; // checks for collison with ground and player
    public string[] animationsArray; // array containing the name of all animations that would stop the enemy from moving

    // private varibales
    private Rigidbody2D EnemyRigidbody;
    private Animator EnemyAnimator;
    private EnemyAggro enemyAggro; // script that manages the aggro state of enemies
    private AnimationChecker animationsChecker; // class containing functions to check which animations are running
    private GameObject Player; // player gameobject is required for navigation when aggroed
    private float movementSpeed, // current movement speed
                  attackRange, // when player is within attack range, enemy stops moving
                  direction = 1.0f, // direction the enemy is facing
                  distance, // distance between the player and the enemy in 2 dimension
                  xDistance; // distance between the player and the enemy on the x axis
    private bool isAggroed, checkNav;

    // Start is called before the first frame update
    void Start()
    {
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
        enemyAggro = GetComponent<EnemyAggro>();
        animationsChecker = GetComponent<AnimationChecker>();

        Player = GameObject.FindWithTag("Player");

        movementSpeed = standardMovementSpeed; // set movement speed

        attackRange = gameObject.GetComponent<EnemyMeleeAttack>().attackRange; // fetch attack range to determine how close the enemy should get to the player to attack
    }

    // Update is called once per frame
    void Update()
    {
        isAggroed = enemyAggro.GetIsAggroed();

        if (isAggroed)
        {
            MoveTowardsPlayer(); // if aggroed guides enemy movement
            Debug.Log("aaaaaaaaaaa");
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        checkNav = NavCollider.IsTouchingLayers(LayerMask.GetMask("Ground")); // check if can walk

        if (!checkNav && !isAggroed)
            direction = -direction;
    }
}
