using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    // public variables
    public float attackRange, 
                 XOffset,
                 YOffset,
                 xVelocity, 
                 yVelocity,
                 maxCooldown; // second the enemy needs to wait until he can attack again
    public GameObject Projectile; // Projectiles shot by rnaged attacks are separate prefabs
    public string[] animationsArray;

    // private variables
    private Animator EnemyAnimator;
    private Rigidbody2D ProjectileRB;
    private float cooldown, newXOffset, newXVelocity;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            CheckRange(); // if attack is of cooldown, check range
        }
    }

    private void CheckRange() // check if player is within attack range
    {
        Collider2D PlayerCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));

        if (PlayerCollider && !CheckEnemyAnimations())
        {
            EnemyAnimator.SetTrigger("IsShooting");
            cooldown = maxCooldown;
        }
    }

    private void SpawnProjectile() // called by the ranged attack animation
    {
        GameObject NewProjectile = Instantiate(Projectile) as GameObject;

        newXOffset = XOffset; 
        newXVelocity = xVelocity;

        if (transform.rotation.eulerAngles.y == 0)
        { // if the player is looing left flip everything
            newXOffset = -newXOffset;
            newXVelocity = -xVelocity;

            NewProjectile.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        Vector2 spawnPosition = new Vector3(transform.position.x + newXOffset, transform.position.y + YOffset, 0);

        NewProjectile.transform.position = spawnPosition;

        ProjectileRB = NewProjectile.GetComponent<Rigidbody2D>();
        ProjectileRB.velocity = new Vector2(newXVelocity, yVelocity);
    }

    private bool CheckEnemyAnimations() // check if an animation is playing, which would prevent the enemy from blocking
    {
        AnimatorStateInfo stateInfo = EnemyAnimator.GetCurrentAnimatorStateInfo(0);

        foreach (string animationName in animationsArray)
        {
            if (stateInfo.IsName(animationName))
            {
                return true; // Return true if an animation in the array is currently playing
            }
        }

        return false;
    }
}
