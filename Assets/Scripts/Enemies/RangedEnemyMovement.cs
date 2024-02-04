using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemyMovement : MonoBehaviour
{
    // public variables
    public float aggroRange; // when player moves within aggro range, enemy begins attacking
    public string[] animationsArray; // array containing the name of all animations that would stop the enemy from moving

    // private varibales
    private Animator EnemyAnimator;
    private AnimationChecker animationsChecker; // class containing functions to check which animtions are running
    private GameObject Player; // player gameobject is required for navigation when aggroed
    private bool isAggroed;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        animationsChecker = GetComponent<AnimationChecker>();

        Player = GameObject.FindWithTag("Player");
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
