using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject spawnItem;
    public float frequency;
    public float initialSpeed;
    private float lastSpawnTime;
    public float despawnTime = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastSpawnTime + frequency)
        {
            Spawn();
            lastSpawnTime = Time.time;
        }
    }

    public void Spawn()
    {
        GameObject newSpawnedObject = Instantiate(
            spawnItem,
            transform.position,
            Quaternion.identity
        );

        newSpawnedObject.transform.parent = transform;
        //make sure pick up code destroys the object
        Destroy(newSpawnedObject, despawnTime);
    }
}
