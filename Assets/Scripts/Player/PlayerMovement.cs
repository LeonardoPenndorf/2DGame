using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // serializedField variables
    [SerializeField] float runSpeed, jumpSpeed;

    // private variables
    private Rigidbody2D PlayerRigidbody;
    private Animator PlayerAnimator;
    private BoxCollider2D PlayerCollider;
    private PolygonCollider2D PlayerFeet;

    private float xAxisInput, yAxisInput;


    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody =GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerCollider = GetComponent<BoxCollider2D>();
        PlayerFeet = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();

        Jump();
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
        bool isGrounded = PlayerFeet.IsTouchingLayers(LayerMask.GetMask("Ground")); // check if feet are touching the ground
        PlayerAnimator.SetBool("IsGrounded", isGrounded); // toggle jumping animation

        if ((Input.GetKeyDown(KeyCode.Space)) && isGrounded) // can only jump when grounded
        {
            PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, jumpSpeed); // jump
        }
    }
}
