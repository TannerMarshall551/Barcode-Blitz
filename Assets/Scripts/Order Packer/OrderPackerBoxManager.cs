using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
public class OrderPackerBoxManager : MonoBehaviour
{
    // Drop zones for boxes
    public GameObject unmadeDZ;
    public GameObject openDZ;
    public GameObject completedDZ;
    public GameObject printerDZ;

    // Prefabs for boxes
    public GameObject unmadePrefab;
    public GameObject openPrefab;
    public GameObject completedPrefab;
    public GameObject labelPrefab;

    // Game manager
    public OrderPackerGameManager gameManager;

    public Transform boxHolder; // Container for all spawned boxes in scene

    List<Box> myBoxes; // All boxes in scene

    // Start is called before the first frame update
    void Start()
    {
        if(gameManager == null){
            Debug.LogError("Game Manager not loaded!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // On Destry is call at the end
    void OnDestroy(){
        ClearBoxes();
    }

    public GameObject GetZone(BoxState state){
        switch(state)
        {
            case BoxState.Unmade:
                return unmadeDZ;
            case BoxState.Open:
                return openDZ;
            case BoxState.Completed:
                return completedDZ;
            default:
                return null;
        }
    }

    // Spawns one box
    public void SpawnBox(BoxState state, GameObject dropZoneSpawn = null){
        
        GameObject myPrefab;
        GameObject dz1;
        GameObject dz2;

        // the box spawned depends on the current state
        switch(state)
        {
            case BoxState.Unmade:
                myPrefab = unmadePrefab;
                dz1 = unmadeDZ;
                dz2 = openDZ;
                break;
            case BoxState.Open:
                myPrefab = openPrefab;
                dz1 = openDZ;
                dz2 = completedDZ;
                break;
            default:
                myPrefab = completedPrefab;
                dz1 = completedDZ;
                if(dropZoneSpawn != null){
                    dz2 = dropZoneSpawn;
                }
                else{
                    dz2 = null;
                }
                break;
        }

        GameObject newBoxTemp = Instantiate(myPrefab, transform.position, transform.rotation, boxHolder);
            
        // Make sure next box is valid
        if(newBoxTemp != null){

            ObjectGrabbableWithZones newBox = newBoxTemp.GetComponent<ObjectGrabbableWithZones>();

            // Adds new box if newBox is valid
            if(newBox != null){
                AddBox(newBox, state, dz1, dz2, dropZoneSpawn);
            }
            else{
                Debug.LogWarning("Unmade Box Prefab not a Grabbable Object with Zones!");
            }
        }
        else{
            Debug.LogWarning("Unmade Box Prefab not working!");
        }
    }

    // Spawns a new Label
    public void SpawnLabel(){

        // create the label
        GameObject newLabelTemp = Instantiate(labelPrefab, transform.position, transform.rotation, boxHolder);

        // make sure instantiate worked
        if(newLabelTemp != null){

            // get the ObjectGrabbableWithZones component
            ObjectGrabbableWithZones newLabel = newLabelTemp.GetComponent<ObjectGrabbableWithZones>();

            // make sure ObjectGrabbableWithZones compenent exists
            if(newLabel != null){

                // Get the drop zones and make sure they exist
                DropZone dz1 = printerDZ.GetComponent<DropZone>();

                if(dz1 != null)
                {
                    newLabel.AddDropZone(dz1.gameObject);

                    DropZone dz2 = openDZ.GetComponent<DropZone>();

                    if(dz2 != null)
                    { 
                        newLabel.AddDropZone(dz2.gameObject);
                    }
                }

                // place label in printer drop zone
                newLabel.Drop(dz1);

                // add event listeners
                newLabel.ObjectDropped += ToggleStateOnDrop;
                newLabel.ObjectGrabbed += ToggleStateOnGrab;
            }
            else{
                Debug.LogError("Label prefab has no ObjectGrabbableWithZones component!");
            }
        }
        else{
            Debug.LogError("No Label prefab loaded!");
        }

    }

    // Spawns a set number of unmade boxes
    public void SpawnBoxes(int amount){
        
        // Initialize myBoxes array
        if(myBoxes == null){
            myBoxes = new List<Box>();
        }
        // Spawn all boxes
        for(int i=0; i<amount; i++){
            SpawnBox(BoxState.Unmade);
            SpawnLabel();
        }
    }

    // Adds one box by fully setting up the newbox
    public void AddBox(ObjectGrabbableWithZones newBox, BoxState boxState, GameObject dropzZone1, GameObject dropzZone2 = null, GameObject dropZoneSpawn = null){

        // Get and validate dropzones
        DropZone dz1 = dropzZone1.GetComponent<DropZone>();

        if(dz1 != null)
        {
            newBox.AddDropZone(dz1.gameObject);

            if (dropzZone2 != null)
            {
                DropZone dz2 = dropzZone2.GetComponent<DropZone>();

                if(dz2 != null)
                {
                    newBox.AddDropZone(dz2.gameObject);
                }
            }

            if(dropZoneSpawn != null){ // Placed new box in drop zone spawn
                
                DropZone dzS = dropZoneSpawn.GetComponent<DropZone>();
               
                if (dzS != null)
                {
                    newBox.Drop(dzS);
                }
                else{ // resorts to dz1 if dzS doesnt exist
                    newBox.Drop(dz1);
                }
            }
            else{ // Places new box in drop zone 1
                newBox.Drop(dz1);
            }



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

            GameObject curDZObj = curBox.box.GetCurrentDropZone();
            if (curDZObj != null){
                DropZone curDZ = curDZObj.GetComponent<DropZone>();
                if(curDZ != null){
                    curDZ.Remove(curBox.box);
                }
            }
            // TODO
            // Destroy(curBox.box.gameObject);
        }
    }

    // Gets the box object
    public Box? GetBox(ObjectGrabbableWithZones boxObj){

        foreach(Box box in myBoxes){
            if(box.box == boxObj){
                return box;
            }
        }

        return null;
    }

    // Removes the box object
    public void RemoveBox(Box box){
        
        if(myBoxes.Contains(box)) // confirm box is in our list
        {
            // remove event listener
            box.box.ObjectDropped -= ToggleStateOnDrop;
            box.box.ObjectGrabbed -= ToggleStateOnGrab;

            myBoxes.Remove(box);

            GameObject dzObj = box.box.GetCurrentDropZone();

            // remove box from dropzone
            if(dzObj != null)
            {
                DropZone dropZone = dzObj.GetComponent<DropZone>();

                if(dropZone != null){
                    dropZone.Remove(box.box);
                }
            }
            Destroy(box.box.gameObject);
        }
    }

    // Removes the label object
    public void RemoveLabel(ObjectGrabbableWithZones label){

        // remove event listener
        label.ObjectDropped -= ToggleStateOnDrop;
        label.ObjectGrabbed -= ToggleStateOnGrab;

        GameObject dzObj = label.GetCurrentDropZone();

        // remove label from dropzone
        if(dzObj != null)
        {
            DropZone dropZone = dzObj.GetComponent<DropZone>();

            if(dropZone != null){
                dropZone.Remove(label);
            }
        }
        Destroy(label.gameObject);
    }

    // Toggles the state on box drop
    public void ToggleStateOnDrop(ObjectGrabbableWithZones myObj){

        Box? curBox = GetBox(myObj);
        
        if(curBox != null){ // box

            gameManager.BoxPlaced(myObj);

            DropZone curDZ = curBox.Value.box.GetCurrentDropZone().GetComponent<DropZone>();

            if(curDZ != null) 
            {
                switch(curBox.Value.state)
                {
                    case BoxState.Unmade:
                        
                        if(curDZ == openDZ.GetComponent<DropZone>()){
                            RemoveBox(curBox.Value);
                            SpawnBox(BoxState.Open);
                        }
                        break;
                    case BoxState.Open:
                        break;
                    case BoxState.Completed:
                        break;
                    default:
                        break;
                }
            }
        }
        else{ // label

            gameManager.LabelPlaced();
            CloseBox();

            DropZone curDZ = myObj.GetCurrentDropZone().GetComponent<DropZone>();
            if(curDZ != null) 
            {
                if(curDZ.gameObject == openDZ){
                    RemoveLabel(myObj);
                }
            }
        }
    }

    // closes all boxes in open drop zone
    public void CloseBox(){

        List<Box> tempBoxes = new List<Box>(myBoxes);

        foreach(Box box in tempBoxes){

            if(box.box.GetCurrentDropZone() == openDZ) // box in open drop zone
            {
                RemoveBox(box);
                SpawnBox(BoxState.Completed, openDZ);
            }
        }
    }

    // Toggles the state on box grab
    public void ToggleStateOnGrab(ObjectGrabbableWithZones boxObj){
        // TODO?
    }

    // return all unopen boxes
    public List<GameObject> GetAllUnopenBoxes(){
        return GameObject.FindGameObjectsWithTag("UnmadeBox").ToList();
    }

    // sets the lock state for grabbing and placing for a dropzone for boxes
    public void SetLockedBox(BoxState dropzone, bool CanGrabFrom, bool CanDropInto){
        DropZone dz;
        
        switch(dropzone)
        {
            case BoxState.Unmade:
                dz = unmadeDZ.GetComponent<DropZone>();
                if(dz != null){
                    dz.SetIsLockedGrab(!CanGrabFrom);
                    dz.SetIsLockedDrop(!CanDropInto);
                }
                break;
            case BoxState.Open:
                dz = openDZ.GetComponent<DropZone>();
                if(dz != null){
                    dz.SetIsLockedGrab(!CanGrabFrom);
                    dz.SetIsLockedDrop(!CanDropInto);
                }
                break;
            case BoxState.Completed:
                dz = completedDZ.GetComponent<DropZone>();
                if(dz != null){
                    dz.SetIsLockedGrab(!CanGrabFrom);
                    dz.SetIsLockedDrop(!CanDropInto);
                }
                break;
            default:
                break;
        } 
    }

    // sets the lock state for grabbing and placing for a dropzone for labels
    public void SetLockedPrinter(bool CanGrabFrom, bool CanDropInto){

        DropZone dz = printerDZ.GetComponent<DropZone>();

        if(dz != null){
            dz.SetIsLockedGrab(!CanGrabFrom);
            dz.SetIsLockedDrop(!CanDropInto);
        }
    }

    
}
