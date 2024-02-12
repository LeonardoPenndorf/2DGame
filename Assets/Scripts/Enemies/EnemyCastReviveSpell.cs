using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCastReviveSpell : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] GameObject reviveGameObject;
    [SerializeField] float maxCooldown;

    // private variables
    private float cooldown;

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        if (cooldown <= 0)
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
}
