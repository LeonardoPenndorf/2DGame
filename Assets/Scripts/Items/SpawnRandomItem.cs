using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnRandomItem : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] GameObject[] itemsArray;
    [SerializeField] float spawnChance; // float value between 0 and 1 representing spawn chance
    [SerializeField] Vector2 launchVector; // when spawning iem is laucnhed

    // private variables
    private int randomItemIndex;

    public void SpawnItem() // this function sometimes spawns a random item
    {
        if (Random.value <= spawnChance) // determine based on spawn chance if an item should be spawned
        {
            randomItemIndex = Random.Range(0, itemsArray.Length); // we want a random item from the items array 
            GameObject NewItem = Instantiate(itemsArray[randomItemIndex], transform.position, Quaternion.identity);

            NewItem.layer = LayerMask.NameToLayer("Items");

            Rigidbody2D ItemRB = NewItem.GetComponent<Rigidbody2D>();

            if(randomItemIndex % 2 == 0) // randomly decide if the item should be launched right or left
            {
                ItemRB.velocity = launchVector;
            }
            else
            {
                ItemRB.velocity = new Vector2(-launchVector.x, launchVector.y);
            }
        }
    }
}
