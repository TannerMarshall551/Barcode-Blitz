using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnsProcessorSpawnManager : MonoBehaviour
{
    public GameObject package;
    public GameObject conveyorPoint;

    public float spawnRate = 2f;

    float nextSpawn = 0f;

    private GameObject newBox;
    public float speed;
    private bool arrived;


    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawn)
        {
  
            newBox = (GameObject)GameObject.Instantiate(package, transform.position, Quaternion.identity);
            arrived = false;
             
            ObjectGrabbable objRemove = newBox.GetComponent<ObjectGrabbable>();
            if (objRemove != null)
                Destroy(objRemove);

            nextSpawn = Time.time + spawnRate;
        }
        if(newBox != null)
        {
            if (newBox.transform.position.x > 0 && !arrived)
            {
                newBox.transform.position = Vector3.MoveTowards(newBox.transform.position, conveyorPoint.transform.position, speed * Time.deltaTime);
            }
            else
            {
                arrived = true;
                newBox.AddComponent<ObjectGrabbable>();
            }
        }
        
        
    }
}
