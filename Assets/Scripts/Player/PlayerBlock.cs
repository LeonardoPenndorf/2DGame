using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] KeyCode blockKeyCode; // key that needs to bee pressed to block

    // private variables
    private Animator PlayerAnimator;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TogglePauseMenu.gameIsPaused) return;

        HandleBlockInput();
    }

    private void HandleBlockInput()
    {
        if (Input.GetKeyDown(blockKeyCode))
        {
            PlayerAnimator.SetBool("IsBlocking", true);
        }

        if (Input.GetKeyUp(blockKeyCode))
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
