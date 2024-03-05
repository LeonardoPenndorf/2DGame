using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyHealth : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int maxHealth, 
                         currentHealth;
    [SerializeField] bool canBeRevived; // only certain enenmies can be revived

    // private variables
    private Animator EnemyAnimator;
    private Rigidbody2D EnemyRB;
    private GameObject Hurtbox;
    private SpawnRandomItem SpawnRandomItemComponent;
    private EnemyAggro enemyAggro;
    private EnemyManager enemyManager;
    private StatsManager statsManager;
    private bool isDead = false,
                 isBlocking = false, // some enemies can block
                 isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        EnemyRB = GetComponent<Rigidbody2D>();
        SpawnRandomItemComponent = GetComponent<SpawnRandomItem>();
        enemyAggro = GetComponent<EnemyAggro>();

        Hurtbox = transform.Find("Hurtbox").gameObject;

        currentHealth = maxHealth;

        statsManager = StatsManager.instance;
        enemyManager = EnemyManager.instance;
        enemyManager.RegisterEnemy(gameObject); // add enemy to the enemies list in enemy manager
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && !isDead) Death();
    }

    public void TakeDamage(int damage, Transform hurtboxTransform)
    {
        if (isDead || currentHealth <= 0) return;

        enemyAggro.SetIsAggroed(true);

        // Check if the enemy is not blocking or if the attack is coming from behind.
        if (!isBlocking || !IsAttackComingFromFront(hurtboxTransform))
        {
            if (isBlocking) EnemyAnimator.SetBool("IsBlocking", false);

            currentHealth -= damage;
            EnemyAnimator.SetTrigger("IsHit");
        }
    }

    private void Death()
    {
        isDead = true;

        statsManager.RegisterKill();

        EnemyAnimator.SetTrigger("IsDead");

        SpawnRandomItemComponent.SpawnItem(); // sometimes enemies will spawn items on death

        EnableDisable(false);
    }

    public void SetIsBlocking(bool newState) { isBlocking = newState; }

    public bool GetCanBeRevived() {  return canBeRevived; }

    public bool GetIsDead() {  return isDead; }

    private void EnableDisable(bool newState) // disables components on death, enables them on revive
    {
        if (newState)
        {
            enemyManager.RegisterEnemy(gameObject); // add enemy to the enemies list in enemy manager
        }
        else
        {
            EnemyRB.velocity = Vector3.zero;
            enemyManager.EnemyDied(gameObject); // remove enemy from the enemies list in enemy manager
        }

        Hurtbox.SetActive(newState); // if enemy was killed during its attacking animation the hurtbox collider will be enabled, thus we must deactivate the hurtbox gameobject to prevent issues

        ToggleComponent<EnemyMeleeAttack>(newState);
        ToggleComponent<EnemyMovement>(newState);
        ToggleComponent<EnemyKnockback>(newState);
        ToggleComponent<EnemyCastReviveSpell>(newState);
        if (ToggleComponent<EnemyBlock>(newState))
            EnemyAnimator.SetBool("IsBlocking", false);
    }

    // Helper method to toggle component states
    private bool ToggleComponent<T>(bool newState) where T : Behaviour
    {
        T component = GetComponent<T>();
        if (component != null)
        {
            component.enabled = newState;
            return true; // Indicates the component was found and toggled
        }
        return false; // Indicates the component was not found
    }

    public void TriggerRevive() // trigger revive animation
    {
        if (canBeRevived && isDead) EnemyAnimator.SetTrigger("Revive");
    }

    private IEnumerator Revive() // called at the start of the revieve animation
    {
        yield return new WaitForSeconds(0.5f);
        EnableDisable(true);

        currentHealth = maxHealth / 2;

        isDead = false;
        canBeRevived = false; // enemies may only be revived once
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("KillZone"))
        {
            TakeDamage(10000000, collision.transform);
        }
    }

    public bool IsAttackComingFromFront(Transform hurtboxTrasnform)
    {
        isFacingRight = transform.rotation.y == 0;

        Vector3 attackDirection = hurtboxTrasnform.position - transform.position;

        // Normalize the direction to get either -1 (left) or 1 (right)
        float direction = Mathf.Sign(attackDirection.x);

        if ((isFacingRight && direction < 0) || (!isFacingRight && direction > 0))
        {
            return true; // Attack is coming from the front
        }
        else
        {
            return false; // Attack is coming from behind
        }
    }
}
