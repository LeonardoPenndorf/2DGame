using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance; // Singleton pattern

    private List<GameObject> enemies = new List<GameObject>(); // list containing all enemy game objects

    void Awake()
    {
        // Singleton pattern setup
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(GameObject enemy) // add a new enemy to the enemies list
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void EnemyDied(GameObject enemy) // remove an enemy that died from the enemies list
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public bool AreAllEnemiesDead() // check if the enemies list is empty
    {
        return enemies.Count == 0;
    }
}
