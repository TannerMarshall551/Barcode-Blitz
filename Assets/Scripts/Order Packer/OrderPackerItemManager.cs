using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderPackerItemManager : MonoBehaviour
{
    public List<GameObject> objectsToSpawn; // Array of object prefabs to spawn
    private List<GameObject> dropZones; // Array of spawn points for shelf (loaded on start)

    public GameObject boxDZ; // Drop zone to place items in box
    public GameObject garbadgeDZ; // Drop zone to place items in trash

    public Transform itemHolder; // Envirnment container to hold all spawned items

    // Game manager
    public OrderPackerGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if(gameManager == null){
            Debug.LogError("Game Manager not loaded!");
        }
        if(dropZones == null){
            dropZones = new List<GameObject>(GameObject.FindGameObjectsWithTag("ItemDZ"));
        }
    }

    public void SpawnObjectsOnShelf(){
        if(dropZones == null){
            dropZones = new List<GameObject>(GameObject.FindGameObjectsWithTag("ItemDZ"));
        }
        SpawnObjectsOnShelf(dropZones);
    }

    // Spawns objects on shelf
    void SpawnObjectsOnShelf(List<GameObject> dropZones)
    {
        foreach (GameObject dropZone in dropZones)
        {
            DropZone curDZ = dropZone.GetComponent<DropZone>();
            if(curDZ != null){
                // Select a random object to spawn
                GameObject selectedObjectPrefab = objectsToSpawn[Random.Range(0, objectsToSpawn.Count)];

                GameObject instantiatedObject;

                // Correctly use the prefab to instantiate and adjust its position
                if(itemHolder != null){
                    instantiatedObject = Instantiate(selectedObjectPrefab, dropZone.transform.position, dropZone.transform.rotation, itemHolder);
                }
                else{
                    instantiatedObject = Instantiate(selectedObjectPrefab, dropZone.transform.position, dropZone.transform.rotation);
                }
                
                // Adjust the instantiated object's position based on its collider height
                if (instantiatedObject.GetComponent<Collider>() != null)
                {
                    ObjectGrabbableWithZones obj = instantiatedObject.GetComponent<ObjectGrabbableWithZones>();
                    if (obj != null)
                    {
                        // add grab and drop event listeners to each item
                        obj.ObjectDropped += ItemDropped;
                        obj.ObjectGrabbed += ItemGrabbed; 

                        // add all dropzones to item
                        obj.SetDropZones(dropZones.ToList());
                        
                        // object successfully added to dropzone
                        if(obj.Drop(curDZ) == 0){

                            Color randomColor;
                
                            // Randomly color the object either green or red
                            if(Random.value > 0.1f){
                                randomColor = Color.green;
                                obj.AddDropZone(boxDZ);
                            }
                            else{
                                randomColor = Color.red;
                                obj.AddDropZone(garbadgeDZ);
                            }

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
                    else{
                         Debug.LogWarning("Invalid Item");
                    }
                }
                else{
                     Debug.LogWarning("Invalid Collider");
                }
            }
            else{
                Debug.LogWarning("Invalid DropZone");
            }
        }
    }

    // removes all items from scene
    public void ClearItems(){
        // TODO
    }

    // get all items in scene
    public List<GameObject> GetAllItems(){
        List<GameObject> allItems = new List<GameObject>();

        if(itemHolder != null){
            for(int i=0; i< itemHolder.transform.childCount; i++){
                allItems.Add(itemHolder.transform.GetChild(i).gameObject);
            }
        }

        return allItems;
    }

    // sets the lock state for grabbing and placing for a dropzone for items
    public void LockAllItems(bool CanGrabFrom, bool CanDropInto, bool CanTrash){
        foreach(GameObject curDZObj in dropZones){
            DropZone curDZ = curDZObj.GetComponent<DropZone>();
            if(curDZ != null){
                curDZ.SetIsLockedGrab(!CanGrabFrom);
                curDZ.SetIsLockedDrop(!CanDropInto);
            }
        }
        DropZone trashDZ = garbadgeDZ.GetComponent<DropZone>();
        trashDZ.SetIsLockedDrop(!CanTrash);
    }

    // called when an item is grabbed
    public void ItemGrabbed(ObjectGrabbableWithZones boxObj){
        gameManager.ItemGrabbed(boxObj);
    }

    // called when an items is dropped
    public void ItemDropped(ObjectGrabbableWithZones boxObj){
        gameManager.ItemPlaced(boxObj);
    }

    // Removes the box object
    public void RemoveItem(ObjectGrabbableWithZones item){
        
        // remove event listeners
        item.ObjectDropped -= ItemDropped;
        item.ObjectGrabbed -= ItemGrabbed;

        GameObject dzObj = item.GetCurrentDropZone();
        if(dzObj != null){
            DropZone dropZone = dzObj.GetComponent<DropZone>();
            if(dropZone != null){
                dropZone.Remove(item);
            }
        }
        Destroy(item.gameObject);
    }

}

