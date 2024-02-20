using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] float healPercentage;
    [SerializeField] int cost, // prize needed to pay to purchase upgrades
                         damageUpgradeAmount, 
                         healthUpgradeAmount;
    [SerializeField] Text CostText, 
                          UpgradedamageText, 
                          UpgradeHealthText, 
                          HealText;

    // private variables
    private PlayerManager playerManager;
    private PlayerHealth playerHealth;
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance;
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        canvas = GetComponentInParent<Canvas>();

        UpdateDescription();
    }

    public void UpgradeDamage()
    {
        if (playerManager.GetDiamonds() < cost || !canvas.enabled) return;
        
        playerManager.UpgradeDamage(damageUpgradeAmount);
        Pay();
        UpdateDescription();
    }

    public void UpgradeHealth()
    {
        if (playerManager.GetDiamonds() < cost || !canvas.enabled) return;

        playerManager.UpgradeHealth(healthUpgradeAmount);
        Pay();
        UpdateDescription();
    }

    public void Heal()
    {
        if (playerManager.GetDiamonds() < cost || !canvas.enabled) return;

        int healAmount = (int)(playerManager.GetMaxHealth() * healPercentage);
        playerHealth.Heal(healAmount);

        Pay();
        UpdateDescription();
    }

    private void Pay()
    {
        playerManager.AdjustDiamonds(-cost);
        cost++;
    }

    public void UpdateDescription()
    {
        CostText.text = $"Cost: {cost}";
        UpgradedamageText.text = $"Damage: {playerManager.GetDamage()}\r\nUpgrade Amount: +{damageUpgradeAmount}";
        UpgradeHealthText.text = $"Max Health: {playerManager.GetMaxHealth()}\r\nUpgrade Amount: +{healthUpgradeAmount}";
        HealText.text = $"Health: {playerManager.GetPlayerHealth()}\r\nHeal Percentage: {healPercentage * 100}%";
    }
}
