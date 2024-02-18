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
    [SerializeField] float regenSpeed; // regen speed of the health bar slider

    // private variables
    private PlayerManager playerManager;
    private Image healthbarBoder;
    private Slider healthbarSlider;
    private int maxHealth, currentHealth, difference;
    private float healthPercentage;
    private bool isScaling;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance; // there is only ever a single player manager instance
        healthbarSlider = GetComponent<Slider>();
        healthbarBoder = transform.Find("Border").GetComponent<Image>();

        SetMaxHealth(playerManager.GetMaxHealth());
        SetSliderValue(playerManager.GetPlayerHealth());
    }

    private void Update()
    {
        if ((!isScaling))
            AdjustSliderValue(playerManager.GetPlayerHealth());

        UpdateHealthBorder();
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        healthbarSlider.maxValue = newMaxHealth;
        maxHealth = newMaxHealth;
    }

    public void SetSliderValue(int health) 
    { 
        healthbarSlider.value = health; 
        currentHealth = health;
    }

    private void AdjustSliderValue(int health)
    {
        difference = Mathf.Abs(currentHealth - health); // difference between the current health value and the new one

        if (health != currentHealth)
        {
            StartCoroutine(ScaleHealth(difference, health > currentHealth ? 1 : -1));
        }
    }

    private void UpdateHealthBorder()
    {
        healthPercentage = (float)currentHealth / maxHealth;
        
        healthbarBoder.sprite = borders[GetSpriteIndex()];
    }

    private int GetSpriteIndex() // Determine which sprite to use based on the current health percentage

    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (healthPercentage >= thresholds[i])
            {
                return i; 
            }
        }

        return borders.Length - 1;
    }

    private IEnumerator ScaleHealth(int difference, int modifier)
    {
        isScaling = true;

        for (int i = 0; i < difference; i++)
        {
            healthbarSlider.value = currentHealth + modifier; // modifier is either a +1 or a -1
            yield return new WaitForSeconds(regenSpeed);
            currentHealth = (int)healthbarSlider.value;
        }

        isScaling = false;
    }
}