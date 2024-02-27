using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerJumpDown : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] GameObject PlayerFeet;
    [SerializeField] Rigidbody2D PlayerRB;
    [SerializeField] float disableTime;

    // private variables
    private AnimationChecker animationsChecker; // class containing functions to check which animations are running
    private TogglePauseGame togglePauseGame;
    private PlayerFeet playerFeetScript;
    private string[] animationsArray; // array containing the name of all animations that would stop you from moving
    private bool isGrounded; // player can only jump down when grounded
    private int playerlayerMask, navigationLayerMask, ignorePlatformsLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        animationsChecker = GetComponent<AnimationChecker>();
        togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();
        playerFeetScript = transform.Find("PlayerFeet").GetComponent<PlayerFeet>();

        animationsArray = GetComponent<PlayerMovement>().animationsArray;

        playerlayerMask = gameObject.layer;
        navigationLayerMask = PlayerFeet.layer;
        ignorePlatformsLayerMask = LayerMask.NameToLayer("IgnorePlatforms");
    }

    public void JumpDown() // Jump down from a platform
    {
        if (togglePauseGame.GetGameIsPaused()) return;

        isGrounded = playerFeetScript.GetIsGrounded();

        if (!animationsChecker.CheckAnimations(animationsArray) && isGrounded)
            StartCoroutine(DisableCollision());
    }

    private IEnumerator DisableCollision() // by briethly turning around the surface arc, the player can jump down
    {
        PlayerFeet.layer = ignorePlatformsLayerMask;
        gameObject.layer = ignorePlatformsLayerMask;
        yield return new WaitForSeconds(disableTime);
        PlayerFeet.layer = navigationLayerMask;
        gameObject.layer = playerlayerMask;
    }
}
