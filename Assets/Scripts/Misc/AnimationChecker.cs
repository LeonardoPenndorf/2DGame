using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChecker : MonoBehaviour
{
    private Animator MyAnimator, PlayerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        MyAnimator = GetComponent<Animator>();
        PlayerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
    }

    public bool CheckAnimations(string[] animationsArray) // check if an animation is playing, which would prevent certain actions
    {
        AnimatorStateInfo stateInfo = MyAnimator.GetCurrentAnimatorStateInfo(0);

        foreach (string animationName in animationsArray)
        {
            if (stateInfo.IsName(animationName))
            {
                return true; // Return true if an animation is currently playing
            }
        }

        return false;
    }

    public bool CheckPlayerIsAttacking(string[] PlayerAnimationsArray) // check if the player is attacking
    {
        AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        foreach (string animationName in PlayerAnimationsArray)
        {
            if (stateInfo.IsName(animationName))
            {
                return true; // Return true if an attack animation is currently playing
            }
        }

        return false;
    }

    public string GetCurrentAnimation() // Returns the name of the currently playing animation
    {
        AnimatorStateInfo stateInfo = MyAnimator.GetCurrentAnimatorStateInfo(0);
        // Loop through all animation clips to find the one that is currently playing
        foreach (AnimationClip clip in MyAnimator.runtimeAnimatorController.animationClips)
        {
            if (stateInfo.IsName(clip.name))
            {
                return clip.name; // Return the name of the currently playing animation
            }
        }
        return ""; // Return an empty string if no animation is found
    }
}
