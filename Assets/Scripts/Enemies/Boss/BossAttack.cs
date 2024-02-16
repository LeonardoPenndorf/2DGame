using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    // public variables
    public int damage;
    public float attackRange;

    // [SerializeField] variables
    [SerializeField] GameObject HurtBox; // melee attack hurt box
    [SerializeField] float maxCooldown;

    // private variables
    private Animator animator;
    private Transform playerTransform;
    private AnimationChecker animationChecker;
    private float cooldown, 
                  distance;
    private string[] animationsArray;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        animationChecker = GetComponent<AnimationChecker>();
        animationsArray = GetComponent<BossFlying>().animationsArray;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        if (cooldown <= 0 && CheckRange() && !animationChecker.CheckAnimations(animationsArray))
            Attack();
    }

    private bool CheckRange()
    {
        distance = Vector3.Distance(playerTransform.position, transform.position);

        return distance <= attackRange;
    }

    private void Attack()
    {
        animator.SetTrigger("IsAttacking");
        cooldown = maxCooldown;
    }

    private void EnableHurtBox()
    {
        HurtBox.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void DisableHurtBox()
    {
        HurtBox.GetComponent<BoxCollider2D>().enabled = false;
    }
}
