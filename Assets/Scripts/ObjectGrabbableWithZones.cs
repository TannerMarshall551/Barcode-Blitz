using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbableWithZones : ObjectGrabbable
{
    public List<GameObject> dropZones; // List of all avaliabe dropzones to use
    public GameObject currentDropZone; // Current dropzone in use
    private Collider objectCollider; // Collider for object

    public bool canPlaceOutsideDropZones; // If you can place the object outside a dropzone

    public delegate void EventHandler(ObjectGrabbableWithZones box); // Eventhandler for object (returns self)
    public event EventHandler ObjectDropped; // Triggered when object is dropped
    public event EventHandler ObjectGrabbed; // Triggered when object is grabbed

    public bool holdingObject = false; // Keeps track if an object is being held

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        objectCollider = GetComponent<Collider>();
    }
    

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // removes collider when holding
        if(currentDropZone != null){
            objectCollider.enabled = false;
        }
        else{
            objectCollider.enabled = true;
        }
    }

    // Getters and setters
    public bool GetCanPlaceOutsideDropZones(){
        return canPlaceOutsideDropZones;
    }

    public void SetCanPlaceOutsideDropZones(bool canPlaceOutsideDropZones){
        this.canPlaceOutsideDropZones = canPlaceOutsideDropZones;
    }

    public GameObject GetCurrentDropZone(){
        return currentDropZone;
    }
    
    public void RemoveCurrentDZ(){
        currentDropZone = null;

    }


    public List<GameObject> GetDropZones(){
        return dropZones;
    }

    public void SetDropZones(List<GameObject> dropZones){
        this.dropZones = new List<GameObject>(dropZones);
    }

    // Adds one drop zone to the list of dropzones
    public void AddDropZone(GameObject dropZone){
        if(!dropZones.Contains(dropZone)){
            dropZones.Add(dropZone);
        }
    }

    // Adds one drop zone to the list of dropzones
    public void RemoveDropZone(GameObject dropZone){
        dropZones.Remove(dropZone);
    }

    public bool GetHoldingObject()
    {
        return holdingObject;
    }

    // Grabs object
    public override void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidBody.useGravity = false;

        objectRigidBody.isKinematic = false;
        currentDropZone = null;

        // Triggers event
        ObjectGrabbed?.Invoke(this);

        holdingObject = true;
    }

    // Drops object
    public int Drop(DropZone dropZone)
    {
        // Attempt to place item in drop zone
        if(dropZone.TryPlace(this) == 0)
        {
            this.objectGrabPointTransform = null;
            objectRigidBody.useGravity = true;

            objectRigidBody.isKinematic = true;
            currentDropZone = dropZone.gameObject;

            // Gets the position element of the dropzone
            Transform transformPosition = currentDropZone.transform.Find("Position");
            if(transformPosition != null)
            {
                // Move object to position
                GameObject position = transformPosition.gameObject;

                this.gameObject.transform.position = position.transform.position;
                this.gameObject.transform.rotation = position.transform.rotation;

                // force physics to update
                Physics.SyncTransforms();

                if(objectCollider == null){
                    objectCollider = GetComponent<Collider>();
                }

                float objectHeight = objectCollider.bounds.size.y;
                // this.transform.position += new Vector3(0, objectHeight / 2, 0);

                holdingObject = false;
            }
            else{
                Debug.Log("Couldn't Find Position of DropZone");
            }

            // Triggers event
            ObjectDropped?.Invoke(this);

            return 0;
        }
        else{
            Debug.Log("Couldn't Place Object");
            return 1;
        }
        
    }

}
