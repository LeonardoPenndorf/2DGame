using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleport : MonoBehaviour
{
    [SerializeField] float maxCooldown, 
                           teleportTime;

    // private variables
    private BoxCollider2D bossCollider;
    private BossFlying bossFlying;
    private Animator animator;
    private AnimationChecker animationChecker;
    private GameObject teleportAnchor; // used to check the boss teleoprts within the bounds of the arena
    private Transform aimTransform; // teleport to the aim transform + offset
    private float cooldown,
                  attackRange,
                  castDistance;
    private string[] animationsArray;
    private bool isPresent = true;
    private Vector3 rectangleSize;

    // Start is called before the first frame update
    void Start()
    {
        bossCollider = GetComponent<BoxCollider2D>();
        bossFlying = GetComponent<BossFlying>();
        animator = GetComponent<Animator>();
        animationChecker = GetComponent<AnimationChecker>();

        teleportAnchor = GameObject.FindGameObjectWithTag("TeleportAnchor");
        aimTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("AimPoint").transform;

        animationsArray = GetComponent<BossFlying>().animationsArray;
        attackRange = GetComponent<BossAttack>().attackRange;
        castDistance = GetComponent<BossCast>().minDistance;
        rectangleSize = teleportAnchor.GetComponent<DrawTeleportSquare>().rectangleSize;

        cooldown = maxCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        if (cooldown <= 0 && !animationChecker.CheckAnimations(animationsArray) && isPresent)
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        isPresent = false;

        animator.SetTrigger("IsTeleporting");
        bossCollider.enabled = false;
        bossFlying.enabled = false;

        StartCoroutine(Reappear());
    }

    private void Attack()
    {
        CalcPosition(true);

        animator.SetTrigger("IsAttacking");
    }

    private void Cast()
    {
        CalcPosition(false);

        animator.SetTrigger("IsCasting");
    }

    private void CalcPosition(bool isAttacking) // calculate the new postion after teleporting
    {
        Vector2 offset = Vector2.zero;
        float maxDistance = isAttacking ? attackRange : castDistance; // Define these variables as needed
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        offset = randomDirection * maxDistance;

        Vector2 newPosition = (Vector2)aimTransform.position + offset;
        
        newPosition = CheckBounds(newPosition); // Ensure newPosition is within bounds and not colliding with obstacles

        transform.position = newPosition;
    }

    private Vector2 CheckBounds(Vector2 newPosition) // ensure the boss teleports within the bounds of the arena
    {
        Vector2 minBounds = teleportAnchor.transform.position - rectangleSize * 0.5f;
        Vector2 maxBounds = teleportAnchor.transform.position + rectangleSize * 0.5f;

        float clampedX = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        return new Vector2(clampedX, clampedY);
    }

    private IEnumerator Reappear()
    {
        yield return new WaitForSeconds(teleportTime);

        int i = Random.Range(0, 2);

        if (i == 0) Attack();
        else Cast();        

        bossCollider.enabled = true;
        bossFlying.enabled = true;
        isPresent = true;

        cooldown = maxCooldown;
    }
}
