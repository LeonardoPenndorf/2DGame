using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    // public variables
    public KeyCode blockKeyCode; // key that needs to bee pressed to block
    // private variables
    private Animator PlayerAnimator;
    private PlayerHealth playerHealth;
    private AnimationChecker animationChecker;
    private string[] animationsArray;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        animationChecker = GetComponent<AnimationChecker>();
        animationsArray = GetComponent<PlayerMovement>().animationsArray;
    }

    // Update is called once per frame
    void Update()
    {
        Block();
    }

    private void Block()
    {
        if (Input.GetKeyDown(blockKeyCode) && !animationChecker.CheckAnimations(animationsArray))
        {
            PlayerAnimator.SetBool("IsBlocking", true);
            playerHealth.SetIsBlocking(true);
        }

        if (Input.GetKeyUp(blockKeyCode))
        {
            PlayerAnimator.SetBool("IsBlocking", false);
            playerHealth.SetIsBlocking(false);
        }
    }
}
