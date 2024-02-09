using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    // private variables
    private Animator PlayerAnimator;
    private PlayerHealth playerHealth;
    private bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void HandleBlockInput(InputAction.CallbackContext context)
    {
        if (TogglePauseMenu.gameIsPaused) return;

        if (context.started)
        {
            PlayerAnimator.SetBool("IsBlocking", true);

        }

        if (context.canceled)
        {
            PlayerAnimator.SetBool("IsBlocking", false);

        }
    }

    public bool IsAttackComingFromFront(Transform hurtboxTrasnform)
    {
        isFacingRight = transform.rotation.y == 0;

        Vector3 attackDirection = hurtboxTrasnform.position - transform.position;

        // Normalize the direction to get either -1 (left) or 1 (right)
        float direction = Mathf.Sign(attackDirection.x);

        if ((isFacingRight && direction > 0) || (!isFacingRight && direction < 0))
        {
            return true; // Attack is coming from the front
        }
        else
        {
            return false; // Attack is coming from behind
        }
    }

    private void StartBlock() // called in the start block animation
    {
        playerHealth.SetIsBlocking(true);
    }

    private void EndBlock() // called in the end block animation
    {
        playerHealth.SetIsBlocking(false);
    }
}
