using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public List<ObjectGrabbableWithZones> objectsInZone;

    public bool isTrash;
    public int maxCapacity;
    public bool hideObjects;

    public Color floorColor;

    // Start is called before the first frame update
    void Start()
    {
        if(objectsInZone == null){
            objectsInZone = new List<ObjectGrabbableWithZones>();
        }
        SetColor(floorColor);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Getters and Setters
    public bool getIsTrash(){
        return isTrash;
    }

    public void setIsTrash(bool isTrash){
        this.isTrash = isTrash;
    }

    public int getMaxCapacity(){
        return maxCapacity;
    }

    public void setMaxCapacitys(int maxCapacity){
        this.maxCapacity = maxCapacity;
    }

    public int getHideObjects(){
        return maxCapacity;
    }

    public void setHideObjects(bool hideObjects){
        this.hideObjects = hideObjects;
    }

    // See if drop zone is full
    public bool IsFull(){
        return objectsInZone.Count == maxCapacity;
    }

    // See if drop zone is full
    public bool IsEmpty(){
        return objectsInZone.Count == 0;
    }


    // Attempts to place object in zone. Returns 0 for success, 1 for failure
    public int TryPlace(ObjectGrabbableWithZones obj){

        if(!IsFull() && obj.GetDropZones().Contains(this.gameObject)){
            objectsInZone.Add(obj);
            return 0;
        } 
        return 1;
    }

    // Attempts to grab object in zone. Returns object for success and null for failure
    public ObjectGrabbableWithZones TryGrab(){

        if(!IsEmpty() && !isTrash){
            int lastObjIndex = objectsInZone.Count - 1;
            ObjectGrabbableWithZones obj = objectsInZone[lastObjIndex];
            objectsInZone.RemoveAt(lastObjIndex);
            return obj;
        } 

        return null;
    }

    // Sets color of drop zone floor based on if its a trash drop zone
    public void SetColor(Color floorColor){

        // Find floor child
        Transform floorTransform = transform.Find("Floor");

        // Make sure the child was found
        if (floorTransform != null)
        {
            // Get the Renderer component from the child
            FloorDZColorChanger floorScript = floorTransform.GetComponent<FloorDZColorChanger>();

            // Check if the child has a Renderer component
            if (floorScript != null)
            {
                floorScript.ChangeColor(floorColor);
                this.floorColor=floorColor;
            }
            else
            {
                Debug.Log("Renderer not found on the child object.");
            }
        }
        else
        {
            Debug.Log("Floor child not found.");
        }
    }
}
