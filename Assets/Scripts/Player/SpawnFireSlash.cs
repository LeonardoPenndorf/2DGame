using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireSlash : MonoBehaviour
{
    // public variables
    public GameObject FireSlash; // sepcial attack spawns a fire slash
    public float xOffset, yOffset; // offset from the player
    public float xVelocity, yVelocity;

    // private variables
    private Rigidbody2D FireSlashRB;
    private float newXOffset, newXVelocity;

    public void SpawnFireSlashMethod()
    {
        GameObject NewFireSlash =  Instantiate(FireSlash) as GameObject;
        newXOffset = xOffset; 
        newXVelocity = xVelocity;

        if (transform.rotation.eulerAngles.y == 180) { // if the player is looing left flip everything
            newXOffset = -newXOffset;
            newXVelocity = -xVelocity;

            NewFireSlash.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        Vector2 spawnPosition = new Vector3(transform.position.x + newXOffset, transform.position.y + yOffset, 0);

        NewFireSlash.transform.position = spawnPosition;

        FireSlashRB = NewFireSlash.GetComponent<Rigidbody2D>();
        FireSlashRB.velocity = new Vector2(newXVelocity, yVelocity);
    }
}
