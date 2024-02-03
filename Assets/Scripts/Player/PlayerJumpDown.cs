using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerJumpDown : MonoBehaviour
{
    // public variables
    public GameObject PlayerFeet;
    public Rigidbody2D PlayerRB;
    public string[] animationsArray; // array containing the name of all animations that would stop you from moving
    public float disableTime;

    // private variables
    private Animator PlayerAnimator;
    private BoxCollider2D PlayerFeetCollider;
    private PlatformEffector2D CurrentPlatformEffector; // alows to jump through a platform from one side
    private PlayerMovement PlayerMovementScript; // We get isGrounded from the movement script
    private AnimationChecker animationsChecker; // class containing functions to check which animations are running
    private bool isGrounded; // player can only jump down when grounded

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerFeetCollider = PlayerFeet.GetComponent<BoxCollider2D>();
        PlayerMovementScript = GetComponent<PlayerMovement>();
        animationsChecker = GetComponent<AnimationChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animationsChecker.CheckAnimations(animationsArray))
        {
            JumpDown();
        }

        if (PlayerRB.velocity.y >= 0 && PlayerRB.velocity.y <= 1 && !isGrounded) // switch to JumpPeak animation when velocity approaches 0
        {
            PlayerAnimator.SetTrigger("IsPeaking");
        }
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
