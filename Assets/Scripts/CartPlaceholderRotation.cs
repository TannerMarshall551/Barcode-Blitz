using UnityEngine;

public class CartPlaceholderRotation : MonoBehaviour
{
    private Rigidbody rb;
    private Quaternion initialRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        // Update the rigidbody's rotation to match the cart's rotation
        rb.MoveRotation(transform.parent.rotation * initialRotation);
    }
}
