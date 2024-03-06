using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // Array of object prefabs to spawn
    public Transform[] SpawnPoint1; // Array of spawn points for shelf 1
    public Transform[] SpawnPoint2; // Array of spawn points for shelf 2

    void Start()
    {
        SpawnObjectsOnShelf(SpawnPoint1);
        SpawnObjectsOnShelf(SpawnPoint2);
    }


    void SpawnObjectsOnShelf(Transform[] spawnPoints)
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
                // Select a random object to spawn
            GameObject selectedObjectPrefab = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
            
            // Correctly use the prefab to instantiate and adjust its position
            GameObject instantiatedObject = Instantiate(selectedObjectPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Adjust the instantiated object's position based on its collider height
            if (instantiatedObject.GetComponent<Collider>() != null)
            {
                Collider collider = instantiatedObject.GetComponent<Collider>();
                float objectHeight = collider.bounds.size.y;
                // This moves the instantiated object up by half its height, properly placing its base at the spawn point.
                instantiatedObject.transform.position += new Vector3(0, objectHeight / 2, 0);
            }

                // Randomly color the object either green or red
            Color randomColor = (Random.value > 0.05f) ? Color.green : Color.red;
            Renderer renderer = instantiatedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = randomColor;
            }
            else
            {
                // Try to find a renderer in the children if the parent doesn't have one
                renderer = instantiatedObject.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = randomColor;
                }
            }
        }
    }
}

