using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public variables
    public float runSpeed, jumpSpeed, gravity;
    public string[] animationsArray; // array containing the name of all animations that would stop you from moving

    // private variables
    private Rigidbody2D PlayerRigidbody;
    private Animator PlayerAnimator;
    private PolygonCollider2D PlayerFeet;

    private float xAxisInput;
    private bool isGrounded, isStunned = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerFeet = GetComponent<PolygonCollider2D>();

        PlayerRigidbody.gravityScale = gravity; // set starting gravity
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned && !CheckAnimations()) // cannot move when stunned or during certain animations
        {
            Run();

            Jump();
        }
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

    private void Jump()
    {
        isGrounded = PlayerFeet.IsTouchingLayers(LayerMask.GetMask("Ground")); // check if feet are touching the ground

        PlayerAnimator.SetBool("IsGrounded", isGrounded); // toggle jumping animation

        if ((Input.GetKeyDown(KeyCode.Space)) && (isGrounded)) // can jump when grounded
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

    private bool CheckAnimations() // check if an animation is playing, which would prevent the player from moving
    {
        AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        foreach(string animationName in animationsArray)
        {
            if (stateInfo.IsName(animationName))
            {
                return true; // Return true if an animation is currently playing
            }
        }

        return false;
    }
}
