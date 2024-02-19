using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    // public varibales
    public float attackRange;

    // [SerializeField] variables
    [SerializeField] Transform spawnPosition;
    [SerializeField] float fovAngle,
                           maxCooldown; // second the enemy needs to wait until he can attack again
    [SerializeField] GameObject Projectile; // Projectiles shot by ranged attacks are separate prefabs
    [SerializeField] string[] animationsArray;

    // private variables
    private Transform Player;
    private Animator EnemyAnimator;
    private EnemyAggro enemyAggro;
    private AnimationChecker animationsChecker; // class containing functions to check which animtions are running
    private EnemyMovement enemyMovement;
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        enemyAggro = GetComponent<EnemyAggro>();
        animationsChecker = GetComponent<AnimationChecker>();
        enemyMovement = GetComponent<EnemyMovement>();

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
        if (DetectPlayerWithFOV() && !animationsChecker.CheckAnimations(animationsArray) && enemyMovement.GetCanAttack())
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
        GameObject NewProjectile = Instantiate(Projectile, spawnPosition.position, Quaternion.identity);
        NewProjectile.GetComponent<Arrow>().direction = transform.rotation.y; // ensure the arrow can only fly in the direction the enemy is facing
    }
}
