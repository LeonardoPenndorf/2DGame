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
        newWave.GetComponent<Arrow>().direction = transform.rotation.y * -1;
    }
}
