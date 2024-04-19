using UnityEngine;

public class PlayerPickupDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    public ObjectGrabbable objectGrabbable;

    public bool isHolding = false;

    public GameObject boxBeingHeld;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
        {
            float pickupDistance = 2f;
            DropZone dropZone;

            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance))
            {
                if (objectGrabbable == null)
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        PickUpObject(objectGrabbable);
                    }
                    else if (raycastHit.transform.TryGetComponent(out dropZone))
                    {
                        objectGrabbable = dropZone.TryGrab();
                        if (objectGrabbable != null)
                        {
                            PickUpObject(objectGrabbable);
                        }
                    }
                }
                else
                {
                    HandleDrop(raycastHit);
                }
            }
        }
    }

    public void PickUpObject(ObjectGrabbable objectToGrab)
    {
        objectGrabbable = objectToGrab;
        objectGrabbable.Grab(objectGrabPointTransform);
        isHolding = true;
        boxBeingHeld = objectGrabbable.gameObject;
    }

    private void HandleDrop(RaycastHit raycastHit)
    {
        DropZone dropZone;
        ObjectGrabbableWithZones curObj = objectGrabbable as ObjectGrabbableWithZones;

        if (curObj != null && raycastHit.transform.TryGetComponent(out dropZone))
        {
            Drop(dropZone);
        }
        else if (curObj != null && curObj.GetCanPlaceOutsideDropZones())
        {
            Drop();
        }
        else
        {
            Drop();
        }
    }

    private void Drop(DropZone dropZone = null)
    {
        if (dropZone != null && objectGrabbable is ObjectGrabbableWithZones curObj && curObj.Drop(dropZone) == 0)
        {
            ClearHeldObject();
        }
        else
        {
            objectGrabbable.Drop();
            ClearHeldObject();
        }
    }

    private void ClearHeldObject()
    {
        objectGrabbable = null;
        isHolding = false;
        boxBeingHeld = null;
    }
}
