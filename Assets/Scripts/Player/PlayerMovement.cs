using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // [SerializeField]  variables
    [SerializeField] GameObject PlayerFeet; // empty game object with a box collider 2D
    [SerializeField] float runSpeed, jumpSpeed, gravity;
    
    // public variables
    public string[] animationsArray; // array containing the name of all animations that would stop you from moving

    // private variables
    private Rigidbody2D PlayerRigidbody;
    private Animator PlayerAnimator;
    private BoxCollider2D PlayerFeetCollider;
    private AnimationChecker animationsChecker; // class containing functions to check which animations are running
    private PlayerHealth playerHealth;
    private TogglePauseGame togglePauseGame;

    private float xAxisInput, 
                  speed;
    private bool isGrounded, 
                 isStunned = false, 
                 facingRight,
                 canRotate = true,
                 reducedMobilty = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerFeetCollider = PlayerFeet.GetComponent<BoxCollider2D>(); 
        animationsChecker = GetComponent<AnimationChecker>();
        playerHealth = GetComponent<PlayerHealth>();
        togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();

        PlayerRigidbody.gravityScale = gravity; // set starting gravity
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = PlayerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || PlayerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")); // check if feet are touching the ground

        PlayerAnimator.SetBool("IsGrounded", isGrounded); // toggle jumping animation

        if (togglePauseGame.GetGameIsPaused()) return;

        xAxisInput = Input.GetAxis("Horizontal"); // get horizontal axis input

        if (!animationsChecker.CheckAnimations(animationsArray)) // cannot move when stunned or during certain animations
        {
            Run();
        }

        if (canRotate)
        {
            Rotate();
        }
    }

    private void Run()
    {
        PlayerAnimator.SetBool("IsRunning", xAxisInput != 0); // set running and idle animations
        speed = reducedMobilty ? runSpeed/1.5f : runSpeed;
        PlayerRigidbody.velocity = new Vector2(xAxisInput * speed, PlayerRigidbody.velocity.y); // move
    }

    private void Rotate()
    {
        if (xAxisInput < 0) // turn left
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (xAxisInput > 0) // turn right
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetCanRotate(bool newState) { canRotate = newState; }

    public void Jump()
    {
        if (togglePauseGame.GetGameIsPaused() || !isGrounded || isStunned || reducedMobilty || animationsChecker.CheckAnimations(animationsArray)) return;

        PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, jumpSpeed); // jump
    }

    public void KnockBack(Vector2 knockbackVector, float stunDuration, Transform enemyTransform)
    {
        if (playerHealth.GetIsDead()) return;

        isStunned = true; // prevent player from moving

        facingRight = transform.localScale.x > 0;

        if (enemyTransform.position.x < transform.position.x)
        {
            PlayerRigidbody.velocity = facingRight ? knockbackVector : new Vector2(-knockbackVector.x, knockbackVector.y);
        }
        else
        {
            PlayerRigidbody.velocity = facingRight ? new Vector2(-knockbackVector.x, knockbackVector.y) : knockbackVector;
        }

        StartCoroutine(Stunned(stunDuration));
    }

    public void EnableRotate() // called in several animations
    {
        SetCanRotate(true);
    }

    public void DisableRotate()  // called in several animations
    {
        SetCanRotate(false);
    }

    private void ReduceMobility() { reducedMobilty = true; } // called at the beginning of the second combo attack, during this attack the player can move a bit

    private void RestoreMobility() { reducedMobilty = false; }


    IEnumerator Stunned(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
    }

    public bool GetIsGrounded() {  return isGrounded; }
}
