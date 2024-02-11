using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // public variables
    public static PlayerManager instance; // Singleton pattern

    // [SerializeField] variables
    [SerializeField] Healthbar healthbar;
    [SerializeField] DiamondsUI diamondsUI;
    [SerializeField] Canvas deathCanvas;
    [SerializeField] int currentHealth, maxHealth; // stores the health of player
    [SerializeField] int currentDiamonds; // amount of diamonds collected by the player

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
        healthbar.SetMaxHealth(maxHealth);
    }

    public void SetPlayerHealth(int health) 
    { 
        currentHealth = health;
        healthbar.SetSliderValue(health);
    }

    public void AdjustPlayerHealth(int health) { currentHealth += health; }

    public void OnPlayerDeath() { deathCanvas.enabled = true; }

    public int GetDiamonds() { return currentDiamonds; }

    public void IncrementDiamonds()
    { 
        currentDiamonds++; 
        diamondsUI.SetDiamonds(currentDiamonds);
    }
}