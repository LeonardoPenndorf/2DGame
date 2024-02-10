using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // [SerializeField] variable
    [SerializeField] int healAmount;
    [SerializeField] AudioClip PickUpSFX;

    // private variables
    private PlayerHealth PlayerHealthComponent;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealthComponent = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // only the player can pick up items
        {
            AudioSource.PlayClipAtPoint(PickUpSFX, Camera.main.transform.position);
            ProcessPickup();
        }
    }

    private void ProcessPickup() // based on the item tag, perform certain functions
    {
        switch (gameObject.tag)
        {
            case "Healing":
                PlayerHealthComponent.Heal(healAmount);
                break;
            case "Diamond":
                PlayerManager.instance.IncrementDiamonds();
                break;
            default:
                Debug.Log("Unhandled pickup tag.");
                return;
        }

        Destroy(gameObject);
    }
}
