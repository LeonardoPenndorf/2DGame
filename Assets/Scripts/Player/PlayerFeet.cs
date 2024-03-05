using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    private Animator playerAnimator;
    private Collider2D navCollider;
    private Rigidbody2D PlayerRB;
    private bool isGrounded;
    private int groundLayerMask, platformsLayerMask;
    private LayerMask groundAndPlatformsLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInParent<Animator>();
        PlayerRB = GetComponentInParent<Rigidbody2D>();
        navCollider = GetComponent<Collider2D>();
        groundAndPlatformsLayerMask = LayerMask.GetMask("Ground", "Platforms");
        groundLayerMask = LayerMask.GetMask("Ground");
        platformsLayerMask = LayerMask.GetMask("Platforms");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerRB.velocity.y >= 0 && PlayerRB.velocity.y <= 1 && !isGrounded) // switch to JumpPeak animation when velocity approaches 0
            playerAnimator.SetTrigger("IsPeaking");

        if (PlayerRB.velocity.y == 0 && isGrounded)
            playerAnimator.SetTrigger("StopPeaking");
    }

    public bool CheckNav()
    {
        return navCollider.IsTouchingLayers(groundLayerMask) || navCollider.IsTouchingLayers(platformsLayerMask);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PlayerRB.velocity.y > 0.5) return;

        if (((1 << collision.gameObject.layer) & groundAndPlatformsLayerMask) != 0)
        {
            isGrounded = true;
            playerAnimator.SetBool("IsGrounded", true); // start jumping animation
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!CheckNav())
        {
            isGrounded = false;
            playerAnimator.SetBool("IsGrounded", false); // end jumping animation
        }
    }

    public bool GetIsGrounded() { return isGrounded; }
}
