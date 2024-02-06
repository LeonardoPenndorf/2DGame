using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // public variables
    public static PlayerManager instance; // Singleton pattern
    public string playerName; // yet to be implemented

    // private varibales
    private int currentHealth, // stores the health of player
                currentDiamonds; // amount of diamonds collected by the player

    private void Awake()
    {
        int ManagerAmount = FindObjectsOfType<PlayerManager>().Length;

        if (ManagerAmount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    public int GetPlayerHealth() { return currentHealth; }

    public void AdjustPlayerHealth(int health)
    {
        currentHealth += health;
    }

    public void OnPlayerDeath()
    {
        Debug.Log("The player died!");
        Destroy(gameObject);
    }

    public int GetDiamonds() { return currentDiamonds; }

    public void IncrementDiamonds() { currentDiamonds++; }
}
