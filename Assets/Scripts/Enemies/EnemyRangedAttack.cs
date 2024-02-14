using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    // public varibales
    public float attackRange;

    // [SerializeField] variables
    [SerializeField] float fovAngle,
                           XOffset,
                           YOffset,
                           xVelocity, 
                           minYVelocity, // y velocity of projectile fluctuates between minYVelocity and maxYVelocity
                           maxYVelocity,
                           maxCooldown; // second the enemy needs to wait until he can attack again
    [SerializeField] GameObject Projectile; // Projectiles shot by ranged attacks are separate prefabs
    [SerializeField] string[] animationsArray;

    // private variables
    private Transform Player;
    private Animator EnemyAnimator;
    private Rigidbody2D ProjectileRB;
    private EnemyAggro enemyAggro;
    private AnimationChecker animationsChecker; // class containing functions to check which animtions are running
    private float cooldown, newXOffset, newXVelocity, newYVelocity;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        enemyAggro = GetComponent<EnemyAggro>();
        animationsChecker = GetComponent<AnimationChecker>();

        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        if (cooldown <= 0 && enemyAggro.GetIsAggroed())
        {
            CheckRange();
        }
    }

    private void CheckRange() // check if player is within attack range
    {
        if (DetectPlayerWithFOV() && !animationsChecker.CheckAnimations(animationsArray))
        {
            EnemyAnimator.SetTrigger("IsShooting");
            cooldown = maxCooldown;
        }
    }

    private bool DetectPlayerWithFOV() // detect enemy with field of view field of view
    {
        Vector2 directionToPlayer = Player.position - transform.position;
        float angleBetweenEnemyAndPlayer = Vector2.Angle(-transform.right, directionToPlayer);

        if (directionToPlayer.magnitude < attackRange && angleBetweenEnemyAndPlayer < fovAngle / 2)
            return true;

        return false;
    }

    private void SpawnProjectile() // called by the ranged attack animation
    {
        GameObject NewProjectile = Instantiate(Projectile) as GameObject;

        newXOffset = XOffset; 
        newXVelocity = xVelocity;
        newYVelocity = Random.Range(minYVelocity, maxYVelocity);

        if (transform.rotation.eulerAngles.y == 0)
        {
            newXOffset = -newXOffset;
            newXVelocity = -xVelocity;

            NewProjectile.transform.rotation = Quaternion.Euler(0.0f, 180.0f, newYVelocity * 2f);
        }

        Vector2 spawnPosition = new Vector3(transform.position.x + newXOffset, transform.position.y + YOffset, 0);

        NewProjectile.transform.position = spawnPosition;

        ProjectileRB = NewProjectile.GetComponent<Rigidbody2D>();
        ProjectileRB.velocity = new Vector2(newXVelocity, newYVelocity);
    }
}
