using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenChest : MonoBehaviour
{
    // private variables
    private GameObject Chest;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
            Chest = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
            Chest = null;
    }

    public void CheckChest(InputAction.CallbackContext context)
    {
        if (!context.performed || Chest == null) return;

        Chest.GetComponent<Chest>().CheckChest();
    }
}
