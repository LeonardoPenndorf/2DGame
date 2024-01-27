using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public variables
    public float playerSpeed, jumpHeight;
    public Rigidbody2D rb; // player rigid body

    // private variables
    private float xAxisInput, yAxisInput;
    private bool isGrounded; // checks if the player is grounded

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xAxisInput = Input.GetAxis("Horizontal"); // get horizontal axis input

        if ((Input.GetKeyDown(KeyCode.Space)) && isGrounded) // can only jump when grounded
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xAxisInput * playerSpeed, rb.velocity.y); // move

        if (rb.velocity.x < 0) // turn right
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (rb.velocity.x > 0) // turn left
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // player is grounded
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // player no longer grounded
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }
}
