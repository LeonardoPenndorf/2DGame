using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemyMovement : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] string[] animationsArray; // array containing the name of all animations that would stop the enemy from moving

    // private varibales
    private EnemyAggro enemyAggro; // script that manages the aggro state of enemies
    private AnimationChecker animationsChecker; // class containing functions to check which animtions are running
    private GameObject Player; // player gameobject is required for navigation when aggroed
    private EnemyKnockback enemyKnockback;
    private bool isAggroed;

    // Start is called before the first frame update
    void Start()
    {
        enemyAggro = GetComponent<EnemyAggro>();
        animationsChecker = GetComponent<AnimationChecker>();
        enemyKnockback = GetComponent<EnemyKnockback>();

        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyKnockback.GetIsStunned()) return; // can't moved when stunned

        isAggroed = enemyAggro.GetIsAggroed();

        if (isAggroed && !animationsChecker.CheckAnimations(animationsArray))
        {
            Rotate();
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
}
