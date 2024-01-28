using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // public variables
    public float standardMovementSpeed, attackRange;
    public BoxCollider2D NavCollider; // checks for collison with ground and player
    public CircleCollider2D AggroCollider; // aggro range

    // private varibales
    private Rigidbody2D EnemyRigidbody;
    private GameObject Player;
    private float movementSpeed, direction = 1.0f;
    private bool isAggroed, checkNav;

    // Start is called before the first frame update
    void Start()
    {
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        AggroCollider = GetComponent<CircleCollider2D>();

        Player = GameObject.FindWithTag("Player");

        movementSpeed = standardMovementSpeed; // set movement speed
    }

    // Update is called once per frame
    void Update()
    {
        if (isAggroed)
        {
            MoveTowardsPlayer(); // if aggroed guides enemy movement
        }

        Move(); // moves and rotates the enemy
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

        if (!checkNav || (Mathf.Abs(Player.transform.position.x - transform.position.x) < attackRange)) // either cannot walk or player within attack range
        {
            movementSpeed = 0;

        }
        else
        {
            movementSpeed = standardMovementSpeed;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAggroed)
            isAggroed = AggroCollider.IsTouchingLayers(LayerMask.GetMask("Player")); // check if player is within aggro range
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        checkNav = NavCollider.IsTouchingLayers(LayerMask.GetMask("Ground")); // check if can walk

        if (!checkNav && !isAggroed)
            direction = -direction;
    }
}
