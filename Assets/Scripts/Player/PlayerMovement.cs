using System.Collections;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // [SerializeField]  variables
    [SerializeField] GameObject PlayerFeet; // empty game object with a box collider 2D
    [SerializeField] KeyCode JumpKeyCode; // key that needs to be pressed to jump
    [SerializeField] float runSpeed, jumpSpeed, gravity;
    
    // public variables
    public string[] animationsArray; // array containing the name of all animations that would stop you from moving

    // private variables
    private Rigidbody2D PlayerRigidbody;
    private Animator PlayerAnimator;
    private BoxCollider2D PlayerFeetCollider;
    private AnimationChecker animationsChecker; // class containing functions to check which animations are running

    private float xAxisInput;
    private bool isGrounded, isStunned = false;

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
        if (TogglePauseMenu.gameIsPaused) return;

        if (!isStunned && !animationsChecker.CheckAnimations(animationsArray)) // cannot move when stunned or during certain animations
        {
            Run();
        }

        isGrounded = PlayerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || PlayerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")); // check if feet are touching the ground

        PlayerAnimator.SetBool("IsGrounded", isGrounded); // toggle jumping animation
    }

    private void Run()
    {
        xAxisInput = Input.GetAxis("Horizontal"); // get horizontal axis input

        PlayerAnimator.SetBool("IsRunning", xAxisInput != 0); // set running and idle animations
        PlayerRigidbody.velocity = new Vector2(xAxisInput * runSpeed, PlayerRigidbody.velocity.y); // move

        if (PlayerRigidbody.velocity.x < 0) // turn left
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (PlayerRigidbody.velocity.x > 0) // turn right
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Jump()
    {
        if (TogglePauseMenu.gameIsPaused) return;

        if (isGrounded && !isStunned && !animationsChecker.CheckAnimations(animationsArray)) // can only jump when grounded
        {
            PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, jumpSpeed); // jump
        }
    }

    public void KnockBack(float knockbackForce, float stunDuration)
    {
        isStunned = true; // prevent player from moving

        Vector2 knockbackVector = new Vector2(knockbackForce, knockbackForce / 2);

        PlayerRigidbody.velocity = knockbackVector * new Vector2(-transform.localScale.x, 1f); // knockback

        StartCoroutine(Stunned(stunDuration));
    }

    IEnumerator Stunned(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
    }

    public bool GetIsGrounded() {  return isGrounded; }
}
