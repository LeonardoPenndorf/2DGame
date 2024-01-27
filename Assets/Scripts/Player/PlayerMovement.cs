using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // serializedField variables
    [SerializeField] float runSpeed, jumpSpeed, climbSpeed, gravity;

    // private variables
    private Rigidbody2D PlayerRigidbody;
    private Animator PlayerAnimator;
    private BoxCollider2D PlayerCollider;
    private PolygonCollider2D PlayerFeet;

    private float xAxisInput, yAxisInput;
    private bool isGrounded, isClimbing;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody =GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerCollider = GetComponent<BoxCollider2D>();
        PlayerFeet = GetComponent<PolygonCollider2D>();

        PlayerRigidbody.gravityScale = gravity; // set starting gravity
    }

    // Update is called once per frame
    void Update()
    {
        Run();

        Jump();

        Climb();
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
        isClimbing = PlayerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")); // check if the player is climbing

        PlayerAnimator.SetBool("IsGrounded", isGrounded); // toggle jumping animation

        if ((Input.GetKeyDown(KeyCode.Space)) && (isGrounded || isClimbing)) // can jump when grounded or climbing
        {
            PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, jumpSpeed); // jump
        }
    }

    private void Climb()
    {
        yAxisInput = Input.GetAxis("Vertical"); // get vertical  axis input

        isClimbing = PlayerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")); // check if the player is climbing
        PlayerAnimator.SetBool("IsClimbing", yAxisInput != 0); // toggle climbing animation

        Debug.Log(isClimbing.ToString());
        if (isClimbing)
        {
            PlayerAnimator.SetBool("IsClimbing", isClimbing); // toggle climbing animation

            PlayerRigidbody.gravityScale = 0;
            PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, climbSpeed * yAxisInput); // climb
        }
        else
        {
            PlayerRigidbody.gravityScale = gravity;
        }
    }
}
