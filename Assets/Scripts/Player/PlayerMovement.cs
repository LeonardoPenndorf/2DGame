using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public variables
    public float runSpeed, jumpSpeed, climbSpeed, gravity;

    // private variables
    private Rigidbody2D PlayerRigidbody;
    private Animator PlayerAnimator;
    private BoxCollider2D PlayerCollider;
    private PolygonCollider2D PlayerFeet;

    private float xAxisInput, yAxisInput;
    private bool isGrounded, touchesClimbing, isClimbing, isStunned = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerCollider = GetComponent<BoxCollider2D>();
        PlayerFeet = GetComponent<PolygonCollider2D>();

        PlayerRigidbody.gravityScale = gravity; // set starting gravity
    }

    // Update is called once per frame
    void Update()
    {

        if (!isStunned) // cannot move when stunned
        {
            Run();

            Jump();

            Climb();
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

        if ((Input.GetKeyDown(KeyCode.Space)) && (isGrounded || isClimbing)) // can jump when grounded or climbing
        {
            PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, jumpSpeed); // jump
        }
    }

    private void Climb()
    {
        ToggleClimb(); // sets isClimbing and gravity

        if (isClimbing)
        {
            yAxisInput = Input.GetAxis("Vertical"); // get vertical  axis input
            PlayerAnimator.SetBool("IsClimbing", yAxisInput != 0); // toggle climbing animation

            PlayerRigidbody.velocity = new Vector2(0, climbSpeed * yAxisInput); // climb
        }
    }

    private void ToggleClimb()
    {
        touchesClimbing = PlayerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")); // check if the player is climbing

        if ((Input.GetKeyDown(KeyCode.Return)) && touchesClimbing) // start or stop climbing by pressing return
        {
            if (!isClimbing) // when not climbing start climbing
            {
                PlayerRigidbody.gravityScale = 0;

                isClimbing = true;
            }
            else // when climbing stop climbing
            {
                PlayerRigidbody.gravityScale = gravity;

                isClimbing = false;
            }
        }

        if (!touchesClimbing || (Input.GetKeyDown(KeyCode.Space))) // if the player no longer touches climbing area or jumps, stop climbing
        {
            PlayerRigidbody.gravityScale = gravity;

            isClimbing = false;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy") && !isStunned) // chek for enemy collision
    //    {
    //        float knockbackForce = collision.gameObject.GetComponent<EnemyStats>().knockbackForce; // get the knockbackforce of the enemy

    //        if (knockbackForce > 0) // check if attack has knockback
    //        {
    //            KnockBack(knockbackForce); // when hit by an attack, the player is knocked back
    //        }
    //    }
    //}

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
}
