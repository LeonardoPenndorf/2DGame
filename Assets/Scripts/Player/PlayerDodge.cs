using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDodge : MonoBehaviour
{
    [SerializeField] float MaxCooldown, holdThreshold;
    [SerializeField] Vector2 dodgeVelocity;

    private Animator PlayerAnimator;
    private PlayerMovement PlayerMovementComponent;
    private TogglePauseGame togglePauseGame;
    private Rigidbody2D rb;
    private float cooldownTimer, rotation;
    private int playerlayerMask, invLayermask;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerMovementComponent = GetComponent<PlayerMovement>();
        togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();
        rb = GetComponent<Rigidbody2D>();

        cooldownTimer = 0;

        playerlayerMask = gameObject.layer;
        invLayermask = LayerMask.NameToLayer("Player_Inv");
    }

    // Update is called once per frame
    void Update()
    {
        if (togglePauseGame.GetGameIsPaused()) return;

        cooldownTimer -= Time.deltaTime;

        if (transform.rotation.eulerAngles.y == 180) rotation = -1;
        else rotation = 1;
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        if (togglePauseGame.GetGameIsPaused() || cooldownTimer > 0 || !context.started) return;

        PlayerAnimator.SetTrigger("Dodge");
    }

    private void StartDodge() // called at the beginning of the dodge animation
    {
        gameObject.layer = invLayermask;

        cooldownTimer = MaxCooldown;
        rb.velocity = dodgeVelocity * new Vector2(rotation, 1);

        PlayerMovementComponent.SetCanRotate(false);
    }

    private void EndDodge() // called at the end of the dodge animation
    {
        gameObject.layer = playerlayerMask;

        cooldownTimer = MaxCooldown;
        rb.velocity = Vector2.zero;

        PlayerMovementComponent.SetCanRotate(true);
    }
}
