using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// States out box can be in
public enum BoxState
{
    Unmade,
    Open,
    Completed
}

// Contains usefull information on each box
public struct Box
{
    public ObjectGrabbableWithZones box;
    public BoxState state;

    public Box(ObjectGrabbableWithZones box, BoxState state)
    {
        this.box = box;
        this.state = state;
    }
}

// Box Manager Class
public class BoxManager : MonoBehaviour
{
    // Drop zones for boxes
    public GameObject unmadeDZ;
    public GameObject openDZ;
    public GameObject completedDZ;

    // Prefabs for boxes
    public GameObject unmadePrefab;

    public Transform boxHolder; // Container for all spawned boxes in scene

    List<Box> myBoxes; // All boxes in scene

    // Start is called before the first frame update
    void Start()
    {
        SpawnBoxes(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawns a set number of unmade boxes
    public void SpawnBoxes(int amount){
        
        // Initialize myBoxes array
        if(myBoxes == null){
            myBoxes = new List<Box>();
        }

        // Make sure unmadeDZ is valid
        DropZone curDZ = unmadeDZ.GetComponent<DropZone>();
        if(curDZ != null){

            // Spawn all boxes
            for(int i=0; i<amount; i++){

                // Spawn next box
                GameObject newBoxTemp = Instantiate(unmadePrefab, transform.position, transform.rotation, boxHolder);
                
                // Make sure next box is valid
                if(newBoxTemp != null){

                    ObjectGrabbableWithZones newBox = newBoxTemp.GetComponent<ObjectGrabbableWithZones>();

                    // Adds new box if newBox is valid
                    if(newBox != null){
                        AddBox(newBox, BoxState.Unmade, unmadeDZ, openDZ);
                    }
                    else{
                        Debug.LogWarning("Unmade Box Prefab not a Grabbable Object with Zones!");
                    }
                }
                else{
                    Debug.LogWarning("Unmade Box Prefab not working!");
                }
            }
        }
        else{
            Debug.LogWarning("Unmade Box DropZone not working!");
        }
    }

    // Adds one box by fully setting up the newbox
    public void AddBox(ObjectGrabbableWithZones newBox, BoxState boxState, GameObject dropzZone1, GameObject dropzZone2 = null){

        // Get and validate dropzones
        DropZone dz1= dropzZone1.GetComponent<DropZone>(); // Must be included
        DropZone dz2= dropzZone2.GetComponent<DropZone>(); // Can be null

        if(dz1 != null)
        {
            newBox.AddDropZone(dz1.gameObject);

            if(dz2 != null)
            {
                newBox.AddDropZone(dz2.gameObject);
            }

            // Places new box in drop zone 1
            newBox.Drop(dz1);

            // Add event actions
            newBox.ObjectDropped += ToggleStateOnDrop;
            newBox.ObjectGrabbed += ToggleStateOnGrab;

            // Create and add the newbox to myBoxes
            Box newBoxObj = new Box(newBox, boxState);
            myBoxes.Add(newBoxObj);
        }
        else{
            Debug.LogWarning("Drop Zone 1 not working");
        }
    }

    // Deletes all boxes
    public void ClearBoxes(){
        foreach(Box curBox in myBoxes){
            curBox.box.ObjectDropped -= ToggleStateOnDrop;
            curBox.box.ObjectGrabbed -= ToggleStateOnGrab;

            // TODO actually destroy the game objects
            // Must remove from the dropzone that the box is currently in
        }
    }

    // Toggles the state on box drop
    public void ToggleStateOnDrop(ObjectGrabbableWithZones boxObj){
        Debug.Log("Box Dropped");
        Debug.Log(boxObj.GetCurrentDropZone());

        // TODO
    }

    // Toggles the state on box grab
    public void ToggleStateOnGrab(ObjectGrabbableWithZones boxObj){
        Debug.Log("Box Grabbed");
        Debug.Log(boxObj.GetCurrentDropZone());

        // TODO
    }





}
