using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlying : MonoBehaviour
{
    // public variables
    public string[] animationsArray;

    // [SerializeField] variables
    [SerializeField] float speed;

    // private variables
    private Animator animator;
    private Transform playerTransform;
    private AnimationChecker animationChecker;
    private Vector3 previousPosition;
    private float attackRange, 
                  distance;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        animationChecker = GetComponent<AnimationChecker>();

        previousPosition = transform.position;
        attackRange = GetComponent<BossAttack>().attackRange; // get attack range from the attack script
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsMoving", transform.position != previousPosition);
        previousPosition = transform.position;

        if (animationChecker.CheckAnimations(animationsArray)) return;

        FlyTowardsPlayer();
        Rotate();
    }

    private void FlyTowardsPlayer()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > attackRange) 
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (playerTransform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
