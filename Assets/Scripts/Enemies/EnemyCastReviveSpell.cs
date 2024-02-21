using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCastReviveSpell : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] GameObject reviveGameObject;
    [SerializeField] float maxCooldown, visionRange;

    // private variables
    private EnemyHealth enemyHealth;
    private TogglePauseGame togglePauseGame;
    private float cooldown;
    private int enemyLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();

        enemyLayerMask = LayerMask.GetMask("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
        if (togglePauseGame.GetGameIsPaused()) return;

        cooldown -= Time.deltaTime;

        if (cooldown <= 0 && FindCorpses())
        {
            CastReviveSpell();
        }
    }

    private void CastReviveSpell()
    {
        GameObject newRevive = Instantiate(reviveGameObject) as GameObject;
        newRevive.transform.position = transform.position;

        cooldown = maxCooldown;
    }

    private bool FindCorpses()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, visionRange, enemyLayerMask); // find enemies

        foreach(Collider2D collider in enemyColliders)
        {
            enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();


            if (enemyHealth != null && enemyHealth.GetIsDead() && enemyHealth.GetCanBeRevived()) return true;
        }

        return false;
    }
}
