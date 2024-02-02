using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerJumpDown : MonoBehaviour
{
    // public variables
    public GameObject PlayerFeet;
    public float disableTime;

    // private variables
    private Animator PlayerAnimator;
    private BoxCollider2D PlayerFeetCollider;
    private PlatformEffector2D CurrentPlatformEffector; // alows to jump through a platform from one side
    private PlayerMovement PlayerMovementScript; // We get isGrounded from the movement script
    private bool isGrounded; // player can only jump down when grounded

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerFeetCollider = PlayerFeet.GetComponent<BoxCollider2D>();
        PlayerMovementScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpDown();
    }

    private void JumpDown() // Jump down from a platform
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            isGrounded = PlayerMovementScript.GetIsGrounded();

            if (CurrentPlatformEffector != null && isGrounded)
            {
                StartCoroutine(DisableCollision());
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PlayerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platforms"))) // check if feet are touching a platform
        {
            CurrentPlatformEffector = collision.gameObject.GetComponent<PlatformEffector2D>(); // fehhc platform effector of the platform, the player is curretnly standing on
        }
    }

    private IEnumerator DisableCollision() // by briethly turning around the surface arc, the player can jump down
    {
        CurrentPlatformEffector.surfaceArc = 0;
        yield return new WaitForSeconds(disableTime);
        CurrentPlatformEffector.surfaceArc = 180;
    }
}
