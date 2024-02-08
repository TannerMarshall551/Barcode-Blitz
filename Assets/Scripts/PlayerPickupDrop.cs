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
        if (Input.GetMouseButtonDown(0))
        {
            if (objectGrabbable == null)
            {
                float pickupDistance = 2f;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                        isHolding = true;
                        boxBeingHeld = objectGrabbable.gameObject;
                    }
                }
            }
            else
            {
                objectGrabbable.Drop();
                objectGrabbable = null;
                isHolding = false;
                boxBeingHeld = null;
            }
        }
    }
}
