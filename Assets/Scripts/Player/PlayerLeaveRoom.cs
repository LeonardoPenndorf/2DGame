using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLeaveRoom : MonoBehaviour
{
    // privatew variables
    private BoxCollider2D PlayerCollider;
    private ExitDoor ExitDoorComponent = null;
    private Animator playerAnimator;
    private PlayerInput playerInput; // when leaving switch input map to disable input
    private PlayerMovement playerMovement; // prevent moving during leave animation

    // Start is called before the first frame update
    void Start()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    
    private void LeaveRoom()
    {
        if (!PlayerCollider.IsTouchingLayers(LayerMask.GetMask("Door")) || !playerMovement.GetIsGrounded())
            return;

        playerAnimator.SetTrigger("Leave");
        playerInput.SwitchCurrentActionMap("DisableMap");
        playerMovement.enabled = false;
    }

    public void CloseDoor() // called at the end of the leave animation
    {
        ExitDoorComponent.StartLoadingRoom();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            ExitDoorComponent = collision.gameObject.GetComponent<ExitDoor>();
        }
    }
}
