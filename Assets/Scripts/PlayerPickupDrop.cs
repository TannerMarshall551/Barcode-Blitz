using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    private ObjectGrabbable objectGrabbable;

    public bool isHolding = false;

    public GameObject boxBeingHeld;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
        {
            
            float pickupDistance = 2f;
            DropZone dropZone;

            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance))
            {
                // not holding anything
                if (objectGrabbable == null)
                {
                    // check for grabbable object
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        Grab();
                    }
                    // check for dropzone
                    else if(raycastHit.transform.TryGetComponent(out dropZone))
                    {
                        objectGrabbable = dropZone.TryGrab();
                        if(objectGrabbable != null){
                            Grab();
                        }
                    }
                }
                // holding something
                else
                {
                    ObjectGrabbableWithZones curObj = objectGrabbable as ObjectGrabbableWithZones;
                    // check if object needs to be placed in zone
                    if(curObj != null)
                    {
                        // check for dropzone
                        if(raycastHit.transform.TryGetComponent(out dropZone))
                        {
                            Drop(dropZone);
                        }
                        // no dropzone, check if object can be placed on ground
                        else if(curObj.GetCanPlaceOutsideDropZones()){
                            Drop();
                        }
                    }
                    // object grabable doesn't need to be placed in zone
                    else{
                        Drop();
                    }
                }
            }
        }
    }

    // Grabs the current object
    private void Grab(){
        objectGrabbable.Grab(objectGrabPointTransform);
        isHolding = true;
        boxBeingHeld = objectGrabbable.gameObject;
    }


    // Drops the current object
    private void Drop(DropZone dropZone = null){
        // check to see if there is a dropzone for the object
        if(dropZone != null){

            ObjectGrabbableWithZones curObj = objectGrabbable as ObjectGrabbableWithZones;

            // attempts to place object in dropzone
            if(curObj.Drop(dropZone) == 0){
                objectGrabbable = null;
                isHolding = false;
                boxBeingHeld = null;
            }
        }
        else{
            objectGrabbable.Drop();
            objectGrabbable = null;
            isHolding = false;
            boxBeingHeld = null;
        }
        
    }
}