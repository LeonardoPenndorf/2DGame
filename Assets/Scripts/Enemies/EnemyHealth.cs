using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int maxHealth, currentHealth, damageReduction;
    [SerializeField] float maxIFrames;

    // private variables
    private Animator EnemyAnimator;
    private Rigidbody2D EnemyRB;
    private BoxCollider2D EnemyCollider;
    private GameObject Hurtbox;
    private SpawnRandomItem SpawnRandomItemComponent;
    private EnemyAggro enemyAggro;
    private float iFrames;
    private bool isBlocking = false; // some enemies can block


    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        EnemyRB = GetComponent<Rigidbody2D>();
        EnemyCollider = GetComponent<BoxCollider2D>();
        SpawnRandomItemComponent = GetComponent<SpawnRandomItem>();
        enemyAggro = GetComponent<EnemyAggro>();

        Hurtbox = transform.Find("Hurtbox").gameObject;

        currentHealth = maxHealth;

        EnemyManager.instance.RegisterEnemy(gameObject); // add enemy to the enemies list in enemy manager
    }

    // Update is called once per frame
    void Update()
    {
        if (iFrames > 0)
        {
            iFrames -= Time.deltaTime;
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(int damage)
    {
        enemyAggro.SetIsAggroed(true);

        if (iFrames <= 0 && currentHealth > 0)
        {
            EnemyAnimator.SetTrigger("IsHit"); // trigger hit animation

            if (!isBlocking)
            {
                currentHealth -= damage;
            }
            else // when blocking subtract damage reduction from damage taken
            {
                currentHealth -= Mathf.Max((damage - damageReduction), 0);
            }

            iFrames = maxIFrames;
        }
    }

    private void Death()
    {
        EnemyRB.velocity = Vector3.zero;
        EnemyRB.bodyType = RigidbodyType2D.Static;
        EnemyAnimator.SetTrigger("IsDead");

        EnemyCollider.enabled = false; // game objects should not collide with dead enemies
        Hurtbox.SetActive(false); // if enemy was killed during its attacking animation the hurtbox collider will be enabled, thus we must deactivate the hurtbox gameobject to prevent issues

        SpawnRandomItemComponent.SpawnItem(); // sometimes enemies will spawn items on death

        EnemyManager.instance.EnemyDied(gameObject); // remove enemy from the enemies list in enemy manager

        GetComponent<EnemyMeleeAttack>().enabled = false;

        EnemyMovement EnemyMovementComponent = GetComponent<EnemyMovement>();
        if(EnemyMovementComponent != null)
        {
            GetComponent<EnemyMovement>().enabled = false;
        }

        RangedEnemyMovement RangedEnemyMovementComponent = GetComponent<RangedEnemyMovement>();
        if (RangedEnemyMovementComponent != null)
        {
            GetComponent<RangedEnemyMovement>().enabled = false;
        }

        EnemyBlock EnemyBlockComponent = GetComponent<EnemyBlock>(); // check if the enemy has the block component
        if(EnemyBlockComponent != null)
        {
            GetComponent<EnemyBlock>().enabled = false;
            EnemyAnimator.SetBool("IsBlocking", false);
        }

        GetComponent<EnemyHealth>().enabled = false;
    }

    public void SetIsBlocking(bool newState)
    {
        isBlocking = newState;
    }
}
