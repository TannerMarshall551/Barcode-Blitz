using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    protected Rigidbody objectRigidBody;
    protected Transform objectGrabPointTransform;

    protected GameObject player;
    protected Transform target;

    protected SmoothRotation smoothRotation;
    protected bool rotationModeActive;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        rotationModeActive = smoothRotation.rotationModeActive;
    }

    public virtual void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidBody.useGravity = false;
    }

    public virtual void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidBody.useGravity = true;
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
}