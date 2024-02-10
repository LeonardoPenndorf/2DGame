using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // public variables
    public static PlayerManager instance; // Singleton pattern

    // [SerializeField] variables
    [SerializeField] int currentHealth, maxHealth; // stores the health of player
    [SerializeField] string playerName; // yet to be implemented

    // private varibales
    private int currentDiamonds; // amount of diamonds collected by the player

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

    public int GetMaxHealth() { return maxHealth; }

    public int GetPlayerHealth() { return currentHealth; }

    public void SetMaxHealth(int health) 
    { 
        maxHealth = health;
        SetPlayerHealth(maxHealth);
    }

    public void SetPlayerHealth(int health) { currentHealth = health; }

    public void AdjustPlayerHealth(int health) { currentHealth += health; }

    public void OnPlayerDeath()
    {
        Debug.Log("The player died!");
        Destroy(gameObject);
    }

    public int GetDiamonds() { return currentDiamonds; }

    public void IncrementDiamonds() { currentDiamonds++; }
}