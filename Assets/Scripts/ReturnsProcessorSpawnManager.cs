using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnsProcessorSpawnManager : MonoBehaviour
{
    public GameObject package, smallPackage, bigPackage;

    public float spawnRate = 2f;

    float nextSpawn = 0f;

    int whatToSpawn;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawn)
        {
            whatToSpawn = Random.Range(1, 4);

            switch(whatToSpawn)
            {
                case 1:
                    Instantiate(smallPackage, transform.position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(package, transform.position, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(bigPackage, transform.position, Quaternion.identity);
                    break;
            }

            nextSpawn = Time.time + spawnRate;
        }
    }
}
