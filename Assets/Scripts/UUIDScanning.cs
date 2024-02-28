using UnityEngine;

/*
 * Checks whether mouse is over Package prefab's label while clicking;
 *  If it is, log UUID of the Package prefab's label
 */

public class UUIDScanning : MonoBehaviour
{
    void Update()
    {
        // Prevent cursor from dissappearing on click – set to always visible
        Cursor.visible = true;
        // 'Unlock' cursor so it can be moved freely on screen
        Cursor.lockState = CursorLockMode.None;

        // Check left mouse button clicked
        if (Input.GetMouseButtonDown(0)) // 0 is number for left mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the ray hits the collider component of plane (label)
                if (hit.collider.gameObject == gameObject)
                {
                    // Log UUID
                    // Check UUID stored in component attached to parent GameObject
                    UUIDGenerator uuidGenerator = hit.collider.GetComponentInParent<UUIDGenerator>();
                    if (uuidGenerator != null)
                    {
                        Debug.Log("UUID: " + uuidGenerator.GetUUID());
                    }
                }
            }
        }
    }
}
