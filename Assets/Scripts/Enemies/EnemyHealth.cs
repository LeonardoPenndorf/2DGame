using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int maxHealth, currentHealth;
    [SerializeField] float maxIFrames, selfDestructDelay;
    [SerializeField] bool canBeRevived; // only certain enenmies can be revived
    [SerializeField] AudioClip hurtSFX, blockSFX, deathSFX;

    // private variables
    private Animator EnemyAnimator;
    private Rigidbody2D EnemyRB;
    private GameObject Hurtbox;
    private SpawnRandomItem SpawnRandomItemComponent;
    private EnemyAggro enemyAggro;
    private float iFrames;
    private bool isDead = false,
                 isBlocking = false; // some enemies can block


    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        EnemyRB = GetComponent<Rigidbody2D>();
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

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }

        if (isDead && (!canBeRevived || !EnemyManager.instance.GetNecromancerPresent()))
        {
            StartCoroutine(SelfDestruct()); // destroy the game object after a short delay
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
                AudioSource.PlayClipAtPoint(hurtSFX, Camera.main.transform.position);
                currentHealth -= damage;
            }
            else // when blocking take no damage
            {
                AudioSource.PlayClipAtPoint(blockSFX, Camera.main.transform.position);
            }

            iFrames = maxIFrames;
        }
    }

    public void Revive()
    {
        if(canBeRevived && isDead)
        {
            EnableDisable(true);

            EnemyAnimator.SetTrigger("Revive");
            currentHealth = maxHealth / 2;

            canBeRevived = false; // enemies may only be revived once

            isDead = false;
        }
    }

    private void Death()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);

        EnemyAnimator.SetTrigger("IsDead");

        SpawnRandomItemComponent.SpawnItem(); // sometimes enemies will spawn items on death

        EnableDisable(false);

        isDead = true;
    }

    public void SetIsBlocking(bool newState)
    {
        isBlocking = newState;
    }

    public bool GetIsDead() {  return isDead; }

    private void EnableDisable(bool newState) // disables components on death, enables them on revive
    {
        if (newState)
        {
            EnemyManager.instance.RegisterEnemy(gameObject); // add enemy to the enemies list in enemy manager
        }
        else
        {
            EnemyRB.velocity = Vector3.zero;
            EnemyManager.instance.EnemyDied(gameObject); // remove enemy from the enemies list in enemy manager
        }

        //EnemyCollider.enabled = newState; // game objects should not collide with dead enemies
        Hurtbox.SetActive(newState); // if enemy was killed during its attacking animation the hurtbox collider will be enabled, thus we must deactivate the hurtbox gameobject to prevent issues

        EnemyMeleeAttack EnemyMeleeAttackComponent = GetComponent<EnemyMeleeAttack>();
        if (EnemyMeleeAttackComponent != null)
        {
            GetComponent<EnemyMeleeAttack>().enabled = newState;
        }

        EnemyMovement EnemyMovementComponent = GetComponent<EnemyMovement>();
        if (EnemyMovementComponent != null)
        {
            GetComponent<EnemyMovement>().enabled = newState;
        }

        EnemyKnockback EnemyKnockbackComponent = GetComponent<EnemyKnockback>();
        if (EnemyKnockbackComponent != null)
        {
            GetComponent<EnemyKnockback>().enabled = newState;
        }

        RangedEnemyMovement RangedEnemyMovementComponent = GetComponent<RangedEnemyMovement>();
        if (RangedEnemyMovementComponent != null)
        {
            GetComponent<RangedEnemyMovement>().enabled = newState;
        }

        EnemyRevive EnemyReviveComponent = GetComponent<EnemyRevive>();
        if (EnemyReviveComponent != null)
        {
            GetComponent<EnemyRevive>().enabled = newState;
        }

        EnemyBlock EnemyBlockComponent = GetComponent<EnemyBlock>(); // check if the enemy has the block component
        if (EnemyBlockComponent != null)
        {
            GetComponent<EnemyBlock>().enabled = newState;
            EnemyAnimator.SetBool("IsBlocking", false);
        }
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructDelay);
        Destroy(gameObject);
    }
}
