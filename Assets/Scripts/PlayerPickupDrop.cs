using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    private ObjectGrabbable objectGrabbable;
    
    private float pickupDistance = 2f;

    public bool isHolding = false;

    public GameObject boxBeingHeld;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
        {
            if (objectGrabbable == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        if(objectGrabbable.CanGrab()){
                            objectGrabbable.Grab(objectGrabPointTransform);
                            isHolding = true;
                            boxBeingHeld = objectGrabbable.gameObject;
                        }
                        else{
                            objectGrabbable = null;
                        }

                    }
                }
            }
            else if(objectGrabbable.HastDropZones())
            {
                Collider heldObjectCollider = boxBeingHeld.GetComponent<Collider>();
                heldObjectCollider.enabled = false;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance))
                {
                    GameObject hitObject = raycastHit.transform.gameObject;
                    if(objectGrabbable.GetDropZones().Contains(hitObject))
                    {
                        objectGrabbable.Drop(hitObject);
                        objectGrabbable = null;
                        isHolding = false;
                        boxBeingHeld = null;
                    }
                }
                heldObjectCollider.enabled = true;
            }
            else{
                objectGrabbable.Drop();
                objectGrabbable = null;
                isHolding = false;
                boxBeingHeld = null;
            }
        }
    }
}
               