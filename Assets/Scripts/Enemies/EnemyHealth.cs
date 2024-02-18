using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] int maxHealth, currentHealth;
    [SerializeField] float selfDestructDelay, // the enemy is destroyed after a short dealy on death
                           fadeDuration; // time it takes the enemy to fade away when it is being destroyed
    [SerializeField] bool canBeRevived; // only certain enenmies can be revived
    [SerializeField] AudioClip hurtSFX, blockSFX, deathSFX;
    [SerializeField] string[] animationsArray;

    // private variables
    private Animator EnemyAnimator;
    private Rigidbody2D EnemyRB;
    private GameObject Hurtbox;
    private SpawnRandomItem SpawnRandomItemComponent;
    private EnemyAggro enemyAggro;
    private SpriteRenderer spriteRenderer;
    private AnimationChecker animChecker;
    private bool coroutineIsRunning = false,
                 isDead = false,
                 isBlocking = false; // some enemies can block


    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        EnemyRB = GetComponent<Rigidbody2D>();
        SpawnRandomItemComponent = GetComponent<SpawnRandomItem>();
        enemyAggro = GetComponent<EnemyAggro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animChecker = GetComponent<AnimationChecker>();

        Hurtbox = transform.Find("Hurtbox").gameObject;

        currentHealth = maxHealth;

        EnemyManager.instance.RegisterEnemy(gameObject); // add enemy to the enemies list in enemy manager
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }

        CheckSelfDestruct();
    }

    public void TakeDamage(int damage)
    {
        if (isDead || currentHealth <= 0) return;

        enemyAggro.SetIsAggroed(true);

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
    }

    private void Death()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);

        EnemyAnimator.SetTrigger("IsDead");

        SpawnRandomItemComponent.SpawnItem(); // sometimes enemies will spawn items on death

        EnableDisable(false);

        isDead = true;
    }

    private void CheckSelfDestruct() // cheks if the enmy should destroy itself
    {
        if (isDead && !coroutineIsRunning && (!canBeRevived || !EnemyManager.instance.GetNecromancerPresent()) && animChecker.CheckAnimations(animationsArray))
            StartCoroutine(SelfDestruct()); // destroy the game object after a short delay
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

        EnemyCastReviveSpell EnemyReviveComponent = GetComponent<EnemyCastReviveSpell>();
        if (EnemyReviveComponent != null)
        {
            GetComponent<EnemyCastReviveSpell>().enabled = newState;
        }

        EnemyBlock EnemyBlockComponent = GetComponent<EnemyBlock>(); // check if the enemy has the block component
        if (EnemyBlockComponent != null)
        {
            GetComponent<EnemyBlock>().enabled = newState;
            EnemyAnimator.SetBool("IsBlocking", false);
        }
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

    private IEnumerator SelfDestruct()
    {
        coroutineIsRunning = true;

        yield return new WaitForSeconds(selfDestructDelay * Random.Range(0.5f, 1.5f));

        float currentTime = 0;

        Color initialColor = spriteRenderer.color;

        while (currentTime < fadeDuration)
        {
            // Calculate the proportion of the fade based on the elapsed time
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);

            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            yield return null;
            currentTime += Time.deltaTime;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("KillZone"))
        {
            TakeDamage(10000000);
        }
    }
}
