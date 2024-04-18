using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnsProcessorBoxManager : MonoBehaviour
{
    public ReturnsProcessorGameManager rpGameManager;

    public GameObject package;
    public GameObject conveyorPoint;

    public GameObject openPackage;
    
    //public float spawnRate = 2f;
    //float nextSpawn = 0f;

    private GameObject newBox;
    private string newBoxUuid;

    public float speed;
    private bool arrived;

    public GameObject dz;
    public GameObject trashDZ;

    DropZone trashDZ1;

    ObjectGrabbableWithZones objGrabZones;
    DropZone dz1;

    private int itemsTrashed = 0;

    public List<GameObject> shelfDropZones;

    private GameObject openBox;

    public GameObject itemDz;
    DropZone itemDz1;

    public void SpawnBox()
    {
        newBox = (GameObject)GameObject.Instantiate(package, transform.position, Quaternion.identity);
        arrived = false;
        newBoxUuid = newBox.GetComponentInChildren<UUIDGenerator>().GetUUID();
        Debug.Log("Just Spawned Box newBoxUUID: " + newBoxUuid);

        SetGameManagerInScanner(rpGameManager, newBox);

        ObjectGrabbableWithZones objRemove = newBox.GetComponent<ObjectGrabbableWithZones>();
        if (objRemove != null)
        {
            Destroy(objRemove);
        }
            

    }

    public void SetGameManagerInScanner(ReturnsProcessorGameManager gameManager, GameObject box)
    {
        UUIDScanner boxScanner = box.GetComponentInChildren<UUIDScanner>();
        boxScanner.rpGameManager = gameManager;
    }

    public void DzSetLockDropGrab(bool canDrop, bool canGrab)
    {
        if (dz1 != null)
        {
            dz1.SetIsLockedDrop(canDrop);
            dz1.SetIsLockedGrab(canGrab);
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

    public void OpenBox(GameObject randomToy, int randomBinIndex)
    {
        dz1.Remove(objGrabZones);
        Destroy(newBox);
        newBoxUuid = null;

        GameObject randomBinDZ = shelfDropZones[randomBinIndex];
        DropZone randomBinDZ1 = randomBinDZ.GetComponent<DropZone>();

        openBox = (GameObject)GameObject.Instantiate(openPackage, dz.transform.position, Quaternion.identity);
        //openBox.GetComponent<Collider>().enabled = true;

        ObjectGrabbableWithZones openBoxObjGrab = openBox.GetComponent<ObjectGrabbableWithZones>();

        List<GameObject> dzList = new();
        dzList.Add(dz1.gameObject);
        dzList.Add(trashDZ1.gameObject);
        openBoxObjGrab.SetDropZones(dzList);
        

        GameObject toy = (GameObject)GameObject.Instantiate(randomToy, new Vector3(dz.transform.position.x, dz.transform.position.y, dz.transform.position.z), Quaternion.identity);

        ObjectGrabbableWithZones toyObjGrab = toy.GetComponent<ObjectGrabbableWithZones>();
        toyObjGrab.SetDropZones(dzList);
        toyObjGrab.AddDropZone(randomBinDZ1.gameObject);
        toyObjGrab.AddDropZone(itemDz1.gameObject);

        
        openBoxObjGrab.Drop(dz1);
        //toyObjGrab.Drop(dz1);

        openBox.GetComponent<BoxCollider>().enabled = false;
        dz.GetComponent<BoxCollider>().enabled = false;
    }

    public string GetNewBoxUUID()
    {
        return newBoxUuid;
    }

    public void SetTrashLockDrop(bool isLocked)
    {
        if (trashDZ1 != null)
            trashDZ1.SetIsLockedDrop(isLocked);
        else
            Debug.Log("null");
    }

    public bool AllItemsTrashed(int numItemsToTrash)
    {
        if (itemsTrashed >= numItemsToTrash)
        {
            itemsTrashed = 0;
            return true;
        }
        else
            return false;
        
    }

    public void ReenableColliders()
    {
        openBox.GetComponent<BoxCollider>().enabled = true;
        dz.GetComponent<BoxCollider>().enabled = true;
    }

    public void ToggleBinDZ(int randBinIndex, bool lockDrop)
    {
        DropZone shelfDZ = shelfDropZones[randBinIndex].GetComponent<DropZone>();
        shelfDZ.SetIsLockedDrop(lockDrop);
    }

    public void ToggleItemDZ(bool lockDrop, bool lockGrab)
    {
        itemDz1.SetIsLockedDrop(lockDrop);
        itemDz1.SetIsLockedGrab(lockGrab);
    }

    public bool CheckBinDZFull(int randBinIndex)
    {
        DropZone shelfDZ = shelfDropZones[randBinIndex].GetComponent<DropZone>();
        return shelfDZ.IsFull();
    }

    public bool CheckItemDZFull()
    {
        return itemDz1.IsFull();
    }

    public void RemoveItemFromBin(int binIndex)
    {
        GameObject zone = shelfDropZones[binIndex];
        DropZone zoneDZ = zone.GetComponent<DropZone>();
        if (zoneDZ.IsFull())
        {
            List<ObjectGrabbableWithZones> itemsList = zoneDZ.GetObjectsInZone();
            GameObject itemInBin = itemsList[0].gameObject;
            zoneDZ.Remove(itemsList[0]);
            Destroy(itemInBin);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dz1 = dz.GetComponent<DropZone>();
        trashDZ1 = trashDZ.GetComponent<DropZone>();
        itemDz1 = itemDz.GetComponent<DropZone>();

    }

    // Update is called once per frame
    void Update()
    {
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
                    dzList.Add(trashDZ1.gameObject);
                    objGrabZones.SetDropZones(dzList);
                }
            }
        }

        if(trashDZ1.IsFull())
        {
            List<ObjectGrabbableWithZones> itemsList = trashDZ1.GetObjectsInZone();
            GameObject itemInTrash = itemsList[0].gameObject;
            trashDZ1.Remove(itemsList[0]);
            Destroy(itemInTrash);
            itemsTrashed++;
        }
    }
}
