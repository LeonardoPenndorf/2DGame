using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    // private variables
    private Animator PlayerAnimator;
    private PlayerHealth playerHealth;

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

    private void StartBlock() // called in the start block animation
    {
        playerHealth.SetIsBlocking(true);
    }

    private void EndBlock() // called in the end block animation
    {
        playerHealth.SetIsBlocking(false);
    }
}
