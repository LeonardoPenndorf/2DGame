using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] GameObject[] enemyArray;
    [SerializeField] GameObject Diamond;
    [SerializeField] int enemiesAmount, diamondsAmount;
    [SerializeField] float maxXOffset, yOffset, spawnTime;
    [SerializeField] Vector2 SpawnVector;

    // private variables
    private Chest chest;
    private Animator animator;
    private EnemyManager enemyManager;
    private bool unlocked = false,
                 isEmpty = false,
                 triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        chest = GetComponent<Chest>();
        animator = GetComponent<Animator>();
        enemyManager = EnemyManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyManager.AreAllEnemiesDead() || isEmpty || ! unlocked) return;
        
        isEmpty = true;
        animator.SetTrigger("Open");
        StartCoroutine(SpawnDiamonds());
    }

    public void CheckChest()
    {
        if (triggered) return;

        if (!unlocked) StartCoroutine(SummonEnemies());

        triggered = true;
    }

    private IEnumerator SummonEnemies()
    {
        for (int i = 0; i < enemiesAmount; i++)
        {
            yield return new WaitForSeconds(spawnTime);

            GameObject randomEnemy = enemyArray[Random.Range(0, enemyArray.Length)];

            float offset = Random.Range(-maxXOffset, maxXOffset);
            Vector3 spawnPosition = new Vector3(transform.position.x + offset, transform.position.y +  yOffset, transform.position.z);

            Instantiate(randomEnemy, spawnPosition, Quaternion.identity);
        }

        unlocked = true;
    }

    private IEnumerator SpawnDiamonds()
    {
        for (int i = 0; i <diamondsAmount; i++)
        {
            yield return new WaitForSeconds(spawnTime * 0.5f);

            GameObject diamond = Instantiate(Diamond, transform.position, Quaternion.identity);

            Rigidbody2D rb = diamond.GetComponent<Rigidbody2D>();

            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), 1).normalized; // Randomize x direction
            Vector2 force = new Vector2(SpawnVector.x * randomDirection.x, SpawnVector.y) * rb.mass;
            rb.velocity = force;
        }

        chest.enabled = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the Gizmos color to red for visibility

        // Draw a wire cube representing the spawn area
        // Assuming enemies spawn at the ground level where the chest is located
        Vector3 center = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 size = new Vector3(maxXOffset * 2, 1, 1); // Adjust the Y and Z values as needed for your game's scale

        Gizmos.DrawWireCube(center, size);
    }
}
