using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaveRoom : MonoBehaviour
{
    // public variables
    public KeyCode LeaveKeyCode;

    // privatew variables
    private BoxCollider2D PlayerCollider;
    private ExitDoor ExitDoorComponent = null;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LeaveRoom();
    }

    private void LeaveRoom()
    {
        if (!PlayerCollider.IsTouchingLayers(LayerMask.GetMask("Door")))
            return;

        if (Input.GetKeyDown(LeaveKeyCode))
        {
            ExitDoorComponent.StartLoadingRoom();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            ExitDoorComponent = collision.gameObject.GetComponent<ExitDoor>();
        }
    }
}
