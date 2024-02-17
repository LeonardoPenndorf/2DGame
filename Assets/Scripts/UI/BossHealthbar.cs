using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] float regenSpeed; // regen speed of the health bar slider

    // private variables
    private BossHealth bossHealth;
    private Slider healthbarSlider;
    private int currentHealth, difference;
    private bool isScaling;

    // Start is called before the first frame update
    void Start()
    {
        GameObject boss = GameObject.FindWithTag("Boss");
        if (boss == null) return;

        bossHealth = boss.GetComponent<BossHealth>();
        healthbarSlider = GetComponent<Slider>();

        SetMaxHealth(bossHealth.GetMaxHealth());
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth == null) return;

        if ((!isScaling))
            AdjustSliderValue(bossHealth.GetCurrentHealth());
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        healthbarSlider.maxValue = newMaxHealth;
        healthbarSlider.value = newMaxHealth;
        currentHealth = newMaxHealth;
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
