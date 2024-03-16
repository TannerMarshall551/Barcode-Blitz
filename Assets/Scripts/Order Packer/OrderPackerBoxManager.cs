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

    // Prefabs for boxes
    public GameObject unmadePrefab;
    public GameObject openPrefab;
    public GameObject completedPrefab;

    public Transform boxHolder; // Container for all spawned boxes in scene

    List<Box> myBoxes; // All boxes in scene

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // On Destry is call at the end
    void OnDestroy(){
        ClearBoxes();
    }

    // Spawns one box
    public void SpawnBox(BoxState state){
        // TODO switch case for state to pick the prefab and dropzones
        GameObject myPrefab;
        GameObject dz1;
        GameObject dz2;

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
                dz2 = null;
                break;
        }

        GameObject newBoxTemp = Instantiate(myPrefab, transform.position, transform.rotation, boxHolder);
            
        // Make sure next box is valid
        if(newBoxTemp != null){

            ObjectGrabbableWithZones newBox = newBoxTemp.GetComponent<ObjectGrabbableWithZones>();

            // Adds new box if newBox is valid
            if(newBox != null){
                AddBox(newBox, state, dz1, dz2);
            }
            else{
                Debug.LogWarning("Unmade Box Prefab not a Grabbable Object with Zones!");
            }
        }
        else{
            Debug.LogWarning("Unmade Box Prefab not working!");
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
        }
    }

    // Adds one box by fully setting up the newbox
    public void AddBox(ObjectGrabbableWithZones newBox, BoxState boxState, GameObject dropzZone1, GameObject dropzZone2 = null){

        
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
        
        if(myBoxes.Contains(box)){
            box.box.ObjectDropped -= ToggleStateOnDrop;
            box.box.ObjectGrabbed -= ToggleStateOnGrab;
            myBoxes.Remove(box);

            GameObject dzObj = box.box.GetCurrentDropZone();
            if(dzObj != null){
                DropZone dropZone = dzObj.GetComponent<DropZone>();
                if(dropZone != null){
                    dropZone.Remove(box.box);
                }
            }
            Destroy(box.box.gameObject);
        }
    }


    // Toggles the state on box drop
    public void ToggleStateOnDrop(ObjectGrabbableWithZones boxObj){

        Box? curBox = GetBox(boxObj);
        
        if(curBox != null){

            DropZone curDZ = curBox.Value.box.GetCurrentDropZone().GetComponent<DropZone>();

            if(curDZ != null)
            {
                switch(curBox.Value.state)
                {
                    case BoxState.Unmade:
                        RemoveBox(curBox.Value);
                        if(curDZ == openDZ.GetComponent<DropZone>()){
                            SpawnBox(BoxState.Open);
                        }
                        else{
                            SpawnBox(BoxState.Unmade);
                        }
                        break;
                    case BoxState.Open:
                        RemoveBox(curBox.Value);
                        if(curDZ == completedDZ.GetComponent<DropZone>()){
                            SpawnBox(BoxState.Completed);
                        }
                        else{
                            SpawnBox(BoxState.Open);
                        }
                        break;
                    case BoxState.Completed:
                        RemoveBox(curBox.Value);
                        SpawnBox(BoxState.Completed);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // Toggles the state on box grab
    public void ToggleStateOnGrab(ObjectGrabbableWithZones boxObj){
        // TODO?
    }

    public List<GameObject> GetAllUnopenBoxes(){
        return GameObject.FindGameObjectsWithTag("UnmadeBox").ToList();
    }

    public void SetLockedBox(BoxState dropzone, bool isLocked){
        DropZone dz;
        switch(dropzone)
        {
            case BoxState.Unmade:
                dz = unmadeDZ.GetComponent<DropZone>();
                if(dz != null){
                    dz.SetIsLocked(isLocked);
                }
                break;
            case BoxState.Open:
                dz = openDZ.GetComponent<DropZone>();
                if(dz != null){
                    dz.SetIsLocked(isLocked);
                }
                break;
            case BoxState.Completed:
                dz = completedDZ.GetComponent<DropZone>();
                if(dz != null){
                    dz.SetIsLocked(isLocked);
                }
                break;
            default:
                break;
        }
    }





}
