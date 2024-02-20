using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnWave : MonoBehaviour
{
    [SerializeField] GameObject wave;
    [SerializeField] Transform SpawnPoint;
    [SerializeField] Vector2 velocity;

    private Rigidbody2D waveRB;
    private Vector2 newVelocity;

    private void SpawnWave() // called during the special attack animation
    {
        GameObject newWave = Instantiate(wave, SpawnPoint.position, Quaternion.identity) as GameObject;

        waveRB = newWave.GetComponent<Rigidbody2D>();
        newVelocity = velocity;

        if (transform.rotation.eulerAngles.y == 180) // if the player is looking left flip everything
        {
            newWave.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            newVelocity = velocity * new Vector2(-1, 1);
        }

        waveRB.velocity = newVelocity;
    }
}
