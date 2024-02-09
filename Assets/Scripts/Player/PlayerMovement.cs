using System.Collections;
using UnityEditor.Timeline;
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

    private float xAxisInput;
    private bool isGrounded, isStunned = false, facingRight;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerFeetCollider = PlayerFeet.GetComponent<BoxCollider2D>(); 
        animationsChecker = GetComponent<AnimationChecker>();

        PlayerRigidbody.gravityScale = gravity; // set starting gravity
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = PlayerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || PlayerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")); // check if feet are touching the ground

        PlayerAnimator.SetBool("IsGrounded", isGrounded); // toggle jumping animation

        if (TogglePauseMenu.gameIsPaused || isGrounded) return;

        xAxisInput = Input.GetAxis("Horizontal"); // get horizontal axis input

        if (!animationsChecker.CheckAnimations(animationsArray)) // cannot move when stunned or during certain animations
        {
            Run();
        }

        Rotate();
    }

    private void Run()
    {
        PlayerAnimator.SetBool("IsRunning", xAxisInput != 0); // set running and idle animations
        PlayerRigidbody.velocity = new Vector2(xAxisInput * runSpeed, PlayerRigidbody.velocity.y); // move
    }

    private void Rotate()
    {
        if (xAxisInput < 0) // turn left
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (xAxisInput > 0) // turn right
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Jump()
    {
        if (TogglePauseMenu.gameIsPaused || !isGrounded || isStunned || animationsChecker.CheckAnimations(animationsArray)) return;

        PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, jumpSpeed); // jump
    }

    public void KnockBack(Vector2 knockbackVector, float stunDuration, Transform enemyTransform)
    {
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

    IEnumerator Stunned(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
    }

    public bool GetIsGrounded() {  return isGrounded; }
}
