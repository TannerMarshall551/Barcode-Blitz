using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPackerDropZoneManager : MonoBehaviour
{
    private List<GameObject> dropZones;

    // Start is called before the first frame update
    void Start()
    {
        GetDropZones();
        UpdateDZVisability(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Gets all the drop zones in the scene
    public void GetDropZones(){
        dropZones = new List<GameObject>();

        GameObject itemManager = GameObject.Find("ItemManager");
        GameObject boxManager = GameObject.Find("BoxManager");

        if(itemManager!=null)
        {
            if(boxManager!=null)
            {
                foreach (Transform child in itemManager.transform)
                {
                    AddDropZone(child.gameObject);
                }

                foreach (Transform child in boxManager.transform)
                {
                    AddDropZone(child.gameObject);
                }
            }
            else
            {
                Debug.LogWarning("No BoxManager Loaded");
            }
        }
        else
        {
            Debug.LogWarning("No ItemManager Loaded");
        }
    }

    //
    public void AddDropZone(GameObject dropZone){
        DropZone curDz = dropZone.GetComponent<DropZone>();
        if(curDz!=null)
        {
            dropZones.Add(dropZone);
            curDz.ObjectDropped += ObjectDropped;
            curDz.ObjectGrabbed += ObjectGrabbed;
        }
    }

    // 
    public void RemoveDropZones(){
        
    }

    //
    public void RemoveDropZone(GameObject dropZone){

    }

    //
    private void ObjectDropped(ObjectGrabbableWithZones myObj){
        UpdateDZVisability(false);
    }

    //
    private void ObjectGrabbed(ObjectGrabbableWithZones myObj){
        UpdateDZVisability(true);
    }

    // 
    public void UpdateDZVisability(bool isHolding){
    
        foreach(GameObject dropZone in dropZones)
        {
            DropZone curDz = dropZone.GetComponent<DropZone>();

            if(curDz != null)
            {
                
                if(isHolding && !curDz.GetIsLockedDrop() && !curDz.IsFull())
                {
                    curDz.SetVisibility(true);
                }
                else if(!isHolding && !curDz.GetIsLockedGrab() && !curDz.IsEmpty())
                {
                    curDz.SetVisibility(true);
                }
                else{
                    curDz.SetVisibility(false);
                }
            }
        
        }

        
    }
    /** 
    on state completion from game loop:
        If holding item:
            Highlight all open dz to place
        if not holdign item:
            Highlight all open dz to grab

    **/



}
