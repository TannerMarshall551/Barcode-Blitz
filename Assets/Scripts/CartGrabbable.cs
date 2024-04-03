using UnityEngine;

public class CartGrabbable : MonoBehaviour
{
    private GameObject player;
    private Rigidbody cartRigidbody;
    private bool isGrabbed = false;

    void Awake()
    {
        player = GameObject.Find("Player");
        cartRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Camera.main != null && Camera.main.gameObject.activeInHierarchy && Camera.main.enabled)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // Only allow cart to be grabbed by handlebar
                if (hit.collider.CompareTag("Handlebar"))
                {
                    isGrabbed = !isGrabbed;
                    if (isGrabbed)
                    {
                        cartRigidbody.isKinematic = true;
                    }
                    else
                    {
                        cartRigidbody.isKinematic = false;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isGrabbed)
        {
            // Snap cart in front
            Vector3 targetPosition = player.transform.position + player.transform.forward * 1;

            // Center cart
            targetPosition += player.transform.right;

            // Keep cart on ground
            targetPosition.y = cartRigidbody.position.y;
            cartRigidbody.MovePosition(Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * 10f));

            // Rotate cart with player rotation
            Quaternion targetRotation = Quaternion.LookRotation(player.transform.forward);
            cartRigidbody.MoveRotation(Quaternion.Slerp(cartRigidbody.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }
    }

}
