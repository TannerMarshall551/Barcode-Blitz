using UnityEngine;

public class PlaceholderSnapping : MonoBehaviour
{
    private GameObject snappedPackage = null;
    private PlayerPickupDrop playerPickupDrop;

    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        playerPickupDrop = playerObject.GetComponent<PlayerPickupDrop>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals(this.tag) && snappedPackage == null && !playerPickupDrop.boxBeingHeld)
        {
            snappedPackage = collision.gameObject;
            Transform placeholderTransform = transform;
            SnapPackageToPlaceholder(snappedPackage, placeholderTransform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals(this.tag) && snappedPackage != null)
        {
            var packageRigidBody = snappedPackage.GetComponent<Rigidbody>();
            packageRigidBody.constraints = RigidbodyConstraints.None;
            snappedPackage.transform.SetParent(null);
            snappedPackage = null;
        }
    }

    private void SnapPackageToPlaceholder(GameObject package, Transform placeholder)
    {
        var packageBounds = package.GetComponentInChildren<Collider>().bounds;

        // Calculate placeholder size
        var placeholderBounds = GetComponent<Collider>().bounds;
        Vector3 placeholderCenterTop = new Vector3(placeholderBounds.center.x, placeholderBounds.max.y, placeholderBounds.center.z);

        // Find current bottom of package
        float lowestPoint = packageBounds.min.y;

        // Calculate position to snap to
        Vector3 targetPosition = placeholderCenterTop + (Vector3.up * (packageBounds.extents.y - lowestPoint + placeholderBounds.extents.y));

        // Set position and rotation
        snappedPackage.transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        snappedPackage.transform.rotation = transform.rotation;
        snappedPackage.transform.SetParent(placeholder, true);
        var packageRigidBody = package.GetComponent<Rigidbody>();
        if (packageRigidBody != null)
        {
            packageRigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void FixedUpdate()
    {
        if (snappedPackage != null)
        {
            snappedPackage.transform.rotation = transform.rotation;
        }
    }
}