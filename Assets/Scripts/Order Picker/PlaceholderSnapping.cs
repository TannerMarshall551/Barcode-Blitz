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
        var placeholderCollider = placeholder.GetComponent<Collider>();
        var packageCollider = package.GetComponentInChildren<Collider>();

        if (placeholderCollider == null || packageCollider == null)
        {
            Debug.LogError("Missing required colliders on either package or placeholder.");
            return;
        }

        // Get top surface of placeholder
        Vector3 placeholderTop = new Vector3(placeholderCollider.bounds.center.x, placeholderCollider.bounds.max.y, placeholderCollider.bounds.center.z);

        // Calculate offset for bottom of package --> placeholder top
        float packageHeight = packageCollider.bounds.size.y;
        Vector3 targetPosition = placeholderTop + Vector3.up * packageHeight / 2;

        package.transform.position = targetPosition;
        package.transform.rotation = placeholder.rotation;
        package.transform.SetParent(placeholder);

        Rigidbody packageRigidBody = package.GetComponent<Rigidbody>();
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