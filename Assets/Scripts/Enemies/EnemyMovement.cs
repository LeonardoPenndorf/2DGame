using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class EnemyMovement : MonoBehaviour
{
    // public variables
    public float standardMovementSpeed, // movement speed of enemy when not aggroed
                 aggroedMovementSpeed; // some enemies move faster when aggroed
    public string[] animationsArray; // array containing the name of all animations that would stop the enemy from moving
    public bool patrol;

    // private varibales
    private Rigidbody2D EnemyRigidbody;
    private Animator EnemyAnimator;
    private EnemyAggro enemyAggro; // script that manages the aggro state of enemies
    private AnimationChecker animationsChecker; // class containing functions to check which animations are running
    private Transform Player; // player transform is required for navigation when aggroed
    private EnemyKnockback enemyKnockback;
    private EnemyNavigator enemyNavigator;
    private float movementSpeed, // current movement speed
                  attackRange, // when player is within attack range, enemy stops moving
                  direction, // direction the enemy is facing
                  startingDirection,
                  distance, // distance between the player and the enemy in 2 dimension
                  xDistance; // distance between the player and the enemy on the x axis
    private bool isAggroed, 
                 falling = false;

    // Start is called before the first frame update
    void Start()
    {
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
        enemyAggro = GetComponent<EnemyAggro>();
        animationsChecker = GetComponent<AnimationChecker>();
        enemyKnockback = GetComponent<EnemyKnockback>();
        enemyNavigator = GetComponentInChildren<EnemyNavigator>();

        Player = GameObject.FindWithTag("Player").transform;

        movementSpeed = 0; // set movement speed

        Initialize();
    }

    private void FixedUpdate()
    {
        if (EnemyRigidbody.velocity.y < 0) falling = true;
        
        else falling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyKnockback.GetIsStunned()) return; // can't moved when stunned

        isAggroed = enemyAggro.GetIsAggroed();

        if (isAggroed)
        {
            MoveTowardsPlayer(); // if aggroed guides enemy movement
        }
        else if (patrol)
        {
            movementSpeed = standardMovementSpeed; // when loosing aggro stop running as well as following the player
            enemyNavigator.CheckDirection();
        }
        else
        {
            movementSpeed = 0;
            direction = startingDirection; // none patrolling enemies just face the starting direction when not aggroed  
        }

        if (!animationsChecker.CheckAnimations(animationsArray))
        {
            if (!enemyNavigator.GetEnemyInFront()) Move(); // prevent overlap
            else movementSpeed = 0;

            Rotate();
        }
        else EnemyRigidbody.velocity = Vector2.zero;

        EnemyAnimator.SetBool("IsMoving", movementSpeed != 0);
    }

    private void Initialize()
    {
        if (transform.rotation.y == 0) direction = -1;
        else direction = transform.rotation.y;
        startingDirection = direction;

        EnemyRangedAttack EnemyRangedAttackComponent = GetComponent<EnemyRangedAttack>();
        if (EnemyRangedAttackComponent != null)
        {
            attackRange = EnemyRangedAttackComponent.attackRange;
        }
        else
        {
            EnemyMeleeAttack EnemyMeleeAttackComponent = GetComponent<EnemyMeleeAttack>();
            attackRange = EnemyMeleeAttackComponent.attackRange; // fetch attack range to determine how close the enemy should get to the player to attack
        }
    }

    private void Move()
    {
        EnemyRigidbody.velocity = new Vector2(direction * movementSpeed, EnemyRigidbody.velocity.y);
    }

    private void Rotate()
    {
        if (direction > 0) transform.rotation = Quaternion.Euler(0, 180, 0); // turn left
        else if (direction < 0) transform.rotation = Quaternion.Euler(0, 0, 0); // turn right
    }

    private void MoveTowardsPlayer()
    {
        distance = Vector2.Distance(Player.position, transform.position);
        xDistance = Mathf.Abs(Player.position.x - transform.position.x);

        if (!enemyNavigator.CheckNav() || (distance < attackRange) || (xDistance < attackRange)) // either cannot walk or player within attack range
            movementSpeed = 0;     
        else movementSpeed = aggroedMovementSpeed; // when aggroed moves faster

        // checks if player is to the right or to the left
        if (Player.position.x < transform.position.x)
            direction = -1.0f; // turn left
        
        else if (Player.position.x > transform.position.x)
            direction = 1.0f; // turn right
    }

    public void SetDirection() { direction = -direction; }

    public bool GetCanAttack() {  return !falling && !enemyKnockback.GetIsStunned(); }
}
