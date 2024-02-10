using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] Sprite[] borders;
    [SerializeField] float[] thresholds;

    // private variables
    private PlayerManager playerManager;
    private Image healthbarBoder;
    private Slider healthbarSlider;
    private int maxHealth;
    private float healthPercantage;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance; // there is only ever a single player manager instance
        healthbarSlider = GetComponent<Slider>();
        healthbarBoder = transform.Find("Border").GetComponent<Image>();
    }

    private void Update()
    {
        SetMaxHealth(playerManager.GetMaxHealth());
        SetSliderValue(playerManager.GetPlayerHealth());
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        healthbarSlider.maxValue = newMaxHealth;
        maxHealth = newMaxHealth;
    }

    public void SetSliderValue(int health)
    {
        healthbarSlider.value = health;
        healthPercantage = (float)health / maxHealth;

        UpdateHealthBorder(healthPercantage);
    }

    private void UpdateHealthBorder(float healthPercentage)
    {
        // Determine which sprite to use based on the current health percentage
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (healthPercentage >= thresholds[i])
            {
                healthbarBoder.sprite = borders[i];
                return; // Exit the function once the correct sprite is assigned
            }
        }

        // If health percentage does not meet any of the above thresholds, use the last sprite
        healthbarBoder.sprite = borders[borders.Length - 1];
    }

}