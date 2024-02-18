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
    [SerializeField] int currentHealth, maxHealth, damage; // stores the health of player
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

        currentHealth = maxHealth;
    }

    public int GetMaxHealth() { return maxHealth; }

    public int GetPlayerHealth() { return currentHealth; }

    public int GetDamage() { return damage; }

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

    public void SetDamage(int newDamage) { damage = newDamage; }

    public void AdjustPlayerHealth(int health) 
    { 
        currentHealth += health;
        healthbar.SetSliderValue(currentHealth);
    }

    public void OnPlayerDeath() { deathCanvas.enabled = true; }

    public int GetDiamonds() { return currentDiamonds; }

    public void AdjustDiamonds(int amount)
    { 
        currentDiamonds += amount; 
        diamondsUI.SetDiamonds(currentDiamonds);
    }

    public void UpgradeDamage(int amount) { damage += amount; }

    public void UpgradeHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        healthbar.SetMaxHealth(maxHealth);
        healthbar.SetSliderValue(currentHealth);
    }

    public void BuffDamage(int buffAmount, float buffDuration)
    {
        StartCoroutine(BuffCoroutine(buffAmount, buffDuration));
    }
    private IEnumerator BuffCoroutine(int buffAmount, float buffDuration) // temporarily buff damage
    {
        damage += buffAmount;
        yield return new WaitForSeconds(buffDuration);
        damage -= buffAmount;
    }
}