using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyMovement : MonoBehaviour
{
    // public variables
    public float standardMovementSpeed, aggroRange;
    public BoxCollider2D NavCollider; // checks for collison with ground and player
    public string[] animationsArray; // array containing the name of all animations that would stop the enemy from moving

    // private varibales
    private Rigidbody2D EnemyRigidbody;
    private Animator EnemyAnimator;
    private AnimationChecker animationsChecker; // class containing functions to check which animtions are running
    private GameObject Player; // player gameobject is required for navigation when aggroed
    private float movementSpeed, 
                  attackRange, 
                  dangerRange,
                  meleeRange, 
                  direction = 1.0f;
    private bool isAggroed, checkNav;
    private float xDistance; // distance between the enemy and the player on the x axis

    // Start is called before the first frame update
    void Start()
    {
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
        animationsChecker = GetComponent<AnimationChecker>();

        Player = GameObject.FindWithTag("Player");

        movementSpeed = 0; // set movement speed

        attackRange = gameObject.GetComponent<EnemyRangedAttack>().attackRange; // fetch attack range to determine how close the enemy should get to the player to attack

    }

    // Update is called once per frame
    void Update()
    {
        if (isAggroed && !animationsChecker.CheckAnimations(animationsArray))
        {
            Rotate();
        }
        else
        {
            CheckAggro(); // if not, check if player is within aggro range
        }

        EnemyAnimator.SetBool("IsMoving", movementSpeed != 0);
    }

    private void Rotate()
    {
        // checks if player is to the right or to the left
        if (Player.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Player.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
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
}
