using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosSpawnWave : MonoBehaviour
{
    [SerializeField] GameObject wave;

    private Transform spawnTransform;

    // Start is called before the first frame update
    void Start()
    {
        spawnTransform = transform.Find("spawnPoint").transform;
    }

    private void SpawnWave()
    {
        GameObject newWave = Instantiate(wave, spawnTransform.position, Quaternion.identity);

        if (transform.rotation.y == 0)
        {
            newWave.GetComponent<AimForPlayer>().direction = -1;

        }
        else
        {
            newWave.GetComponent<AimForPlayer>().direction = 1;
        }
    }
}
