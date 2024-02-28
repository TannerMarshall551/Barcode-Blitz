using System.Diagnostics;
using UnityEngine;

/*
 * Checks whether mouse is over Package prefab's label while clicking;
 *  If it is, log UUID of the Package prefab's label
 */

public class UUIDScanner : MonoBehaviour
{
    void Update()
    {
        // Check left mouse button clicked
        if (Input.GetMouseButtonDown(1)) // 1 is number for right mouse
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
                        UnityEngine.Debug.Log("UUID: " + uuidGenerator.GetUUID());
                    }
                }
            }
        }
    }
}
