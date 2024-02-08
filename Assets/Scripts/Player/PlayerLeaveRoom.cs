using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaveRoom : MonoBehaviour
{
    // privatew variables
    private BoxCollider2D PlayerCollider;
    private ExitDoor ExitDoorComponent = null;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
    }
    
    public void LeaveRoom()
    {
        if (!PlayerCollider.IsTouchingLayers(LayerMask.GetMask("Door")))
            return;
        
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
