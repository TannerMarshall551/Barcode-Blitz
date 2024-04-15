using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnsProcessorBoxManager : MonoBehaviour
{
    public GameObject package;
    public GameObject conveyorPoint;

    public GameObject openPackage;
    public List<GameObject> toyCars;

    //public float spawnRate = 2f;
    //float nextSpawn = 0f;

    private GameObject newBox;

    public float speed;
    private bool arrived;

    public GameObject dz;
    ObjectGrabbableWithZones objGrabZones;
    DropZone dz1;

    public float chanceGood = 80;

    public void SpawnBox()
    {
        newBox = (GameObject)GameObject.Instantiate(package, transform.position, Quaternion.identity);
        arrived = false;

        ObjectGrabbableWithZones objRemove = newBox.GetComponent<ObjectGrabbableWithZones>();
        if (objRemove != null)
            Destroy(objRemove);

    }

    public void DzSetLockDropGrab(bool canDrop, bool canGrab)
    {
        if (dz1 != null)
        {
            dz1.SetIsLockedDrop(!canDrop);
            dz1.SetIsLockedGrab(!canGrab);
        }
        else
            Debug.Log("null");
    }

    public bool HoldingNewBox()
    {
        if (objGrabZones != null)
        {
            return objGrabZones.GetHoldingObject();
        }
        else
            return false;
    }

    public bool IsDZFull()
    {
        if (dz1 != null)
            return dz1.GetObjectsInZone().Count == 1;
        else
            return false;
    }

    public bool OpenBox()
    {
        Debug.Log("here");

        Destroy(newBox);

        int randomToyIndex = Random.Range(0, 6);

        GameObject randomToy = toyCars[randomToyIndex];

        GameObject openBox = (GameObject)GameObject.Instantiate(openPackage, dz.transform.position, Quaternion.identity);
        //openBox.GetComponent<Collider>().enabled = true;

        ObjectGrabbableWithZones openBoxObjGrab = openBox.GetComponent<ObjectGrabbableWithZones>();

        List<GameObject> dzList = new();
        dzList.Add(dz1.gameObject);
        openBoxObjGrab.SetDropZones(dzList);
        openBoxObjGrab.Drop(dz1);

        GameObject toy = (GameObject)GameObject.Instantiate(randomToy, new Vector3(dz.transform.position.x, dz.transform.position.y, dz.transform.position.z), Quaternion.identity);

        ObjectGrabbableWithZones toyObjGrab = toy.GetComponent<ObjectGrabbableWithZones>();
        toyObjGrab.SetDropZones(dzList);
        //toyObjGrab.Drop(dz1);


        float isCorrect = Random.Range(1, 101);
        Debug.Log("Is Correct: " + isCorrect);
        if (isCorrect > chanceGood)
            return false;
        else
            return true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dz1 = dz.GetComponent<DropZone>();
        /*if (Time.time > nextSpawn)
        {
            SpawnBox();
            nextSpawn = Time.time + spawnRate;
        }*/ 
        if (newBox != null)
        {
            if (newBox.transform.position.x > 0 && !arrived)
            {
                newBox.transform.position = Vector3.MoveTowards(newBox.transform.position, conveyorPoint.transform.position, speed * Time.deltaTime);
            }
            else
            {
                arrived = true;
                ObjectGrabbableWithZones objectGrabbableWithZones = newBox.GetComponent<ObjectGrabbableWithZones>();
                if (objectGrabbableWithZones == null)
                {
                    newBox.AddComponent<ObjectGrabbableWithZones>();
                    objGrabZones = newBox.GetComponent<ObjectGrabbableWithZones>();
                    List<GameObject> dzList = new();
                    dzList.Add(dz1.gameObject);
                    objGrabZones.SetDropZones(dzList);
                }
            }
        }
    }
}
