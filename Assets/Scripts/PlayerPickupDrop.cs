using UnityEngine;

public class PlayerPickupDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    public ObjectGrabbable objectGrabbable;

    public bool isHolding = false;

    public GameObject boxBeingHeld;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
        {
            if (objectGrabbable == null)
            {
                float pickupDistance = 2f;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        PickUpObject(objectGrabbable);
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

    public void PickUpObject(ObjectGrabbable objectGrabbable)
    {
        this.objectGrabbable = objectGrabbable;
        objectGrabbable.Grab(objectGrabPointTransform);
        isHolding = true;
        boxBeingHeld = objectGrabbable.gameObject;
    }
}
