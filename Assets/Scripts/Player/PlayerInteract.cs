using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    // private variables
    private Chest chest;
    private ExitDoor exitDoor;
    private PlayerMovement playerMovement; // prevent moving during leave animation
    private EnemyManager enemyManager;
    private Animator playerAnimator;
    private PlayerInput playerInput; // when leaving switch input map to disable input

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        enemyManager = EnemyManager.instance;
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
            chest = collision.gameObject.GetComponent<Chest>();

        if (collision.gameObject.CompareTag("Door"))
            exitDoor = collision.gameObject.GetComponent<ExitDoor>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
            chest = null;

        if (collision.gameObject.CompareTag("Door"))
            exitDoor = null;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed || !playerMovement.GetIsGrounded() || !enemyManager.AreAllEnemiesDead()) return;

        if (chest != null)
        {
            chest.CheckChest();
        }

        if (exitDoor != null)
        {
            playerAnimator.SetTrigger("Leave");
            playerInput.SwitchCurrentActionMap("DisableMap");
            playerMovement.enabled = false;
        }

    }

    public void CloseDoor() // called at the end of the leave animation
    {
        exitDoor.StartLoadingRoom();
    }
}
