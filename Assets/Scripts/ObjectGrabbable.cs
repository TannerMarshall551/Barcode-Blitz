using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;

    private GameObject player;
    private Transform target;

    private SmoothRotation smoothRotation;
    private bool rotationModeActive;

    public List<GameObject> dropZones;
    public GameObject currentDropZone;
    private bool canGrab = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationModeActive = smoothRotation.rotationModeActive;
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidBody.useGravity = false;

        if(currentDropZone != null){

            DropZone curDropZone = currentDropZone.GetComponent<DropZone>();

            curDropZone.ObjectPickedUp();
            currentDropZone = null;
            objectRigidBody.isKinematic = false;
        }
    }

    public void Drop(GameObject dropZone = null)
    {
        this.objectGrabPointTransform = null;
        objectRigidBody.useGravity = true;

        if(dropZone != null){
            currentDropZone = dropZone;

            DropZone curDropZone = currentDropZone.GetComponent<DropZone>();

            if(curDropZone.CanPickupObjectAgain() == false){
                this.ToggleGrab();
            }
            curDropZone.ObjectPlaced();

            this.gameObject.transform.position = currentDropZone.transform.position;
            this.gameObject.transform.rotation = currentDropZone.transform.rotation;
            objectRigidBody.isKinematic = true;
        }
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
        target = player.GetComponent<Transform>();
        smoothRotation = player.GetComponent<SmoothRotation>();
        rotationModeActive = smoothRotation.rotationModeActive;
        objectRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(objectGrabPointTransform != null) 
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidBody.MovePosition(newPosition);
        }
        if (!objectRigidBody.useGravity && !rotationModeActive)
        {
            transform.LookAt(target);
            objectRigidBody.freezeRotation = false;
        }
        else if (rotationModeActive)
        {
            objectRigidBody.freezeRotation = true;
        }
    }

    public bool HastDropZones(){
        if(dropZones.Count == 0){
            return false;
        }
        else{
            return true;
        }
    }

    public List<GameObject> GetDropZones(){
        return dropZones;
    }

    public bool CanGrab(){
        return canGrab;
    }

    public bool ToggleGrab(){
        return canGrab = !canGrab;
    }
}


