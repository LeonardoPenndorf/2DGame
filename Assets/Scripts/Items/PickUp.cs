using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // [SerializeField] variable
    [SerializeField] int healAmount, buffAmount;
    [SerializeField] float buffDuration;

    // private variables
    private PlayerHealth playerHealth;
    private PlayerManager playerManager;
    private StatsManager statsManager;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        playerManager = PlayerManager.instance;
        statsManager = StatsManager.instance;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // only the player can pick up items
            ProcessPickup();
    }

    private void ProcessPickup() // based on the item tag, perform certain functions
    {
        switch (gameObject.tag)
        {
            case "Healing":
                playerHealth.Heal(healAmount);
                break;
            case "Diamond":
                playerManager.AdjustDiamonds(1);
                statsManager.RegisterDaimond();
                break;
            case "Buff":
                playerManager.BuffDamage(buffAmount, buffDuration);
                break;
            default:
                Debug.Log("Unhandled pickup tag.");
                return;
        }

        Destroy(gameObject);
    }
}
