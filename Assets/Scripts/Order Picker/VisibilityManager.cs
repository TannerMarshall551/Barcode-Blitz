using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : MonoBehaviour
{
    public Transform target; // Dynamically set target
    public LayerMask obstructingLayer; // Layer for obstructing objects
    private List<Renderer> previouslyObstructed = new List<Renderer>(); // Track previously obstructed objects
    public bool isTargetSet = false; // Flag to track if the target has been set

    void LateUpdate()
    {
        if (target == null) return;

        // Reset visibility for previously obstructed objects
        ResetObstructedObjectsVisibility();

        // Calculate the top center point of the target's bounding box
        Vector3 targetTopCenter = target.position + target.up * target.GetComponent<Collider>().bounds.extents.y;

        // Adjust the starting point of the ray to the camera's position
        Vector3 rayStart = transform.position;

        // Calculate the direction from the camera to the top center of the target's bounding box
        Vector3 directionToTargetTop = targetTopCenter - rayStart;

        // Perform raycast towards the top center of the target
        RaycastHit[] hits = Physics.RaycastAll(rayStart, directionToTargetTop.normalized, directionToTargetTop.magnitude, obstructingLayer);

        // Filter and process hits
        ProcessRaycastHits(hits, directionToTargetTop.magnitude);
    }

    private void ProcessRaycastHits(RaycastHit[] hits, float maxDistance)
    {
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == target) continue;
            float distanceToHit = Vector3.Distance(transform.position, hit.point);

            if (distanceToHit < maxDistance)
            {
                HideRenderersRecursively(hit.transform);
            }
        }
    }

    private void ResetObstructedObjectsVisibility()
    {
        foreach (Renderer renderer in previouslyObstructed)
        {
            if (renderer != null)
            {
                renderer.enabled = true; // Reset visibility
            }
        }
        previouslyObstructed.Clear(); // Clear the list after resetting visibility
    }

    public void SetTarget(Transform newTarget)
    {
        if (!isTargetSet) // Check if a target has already been set
        {
            target = newTarget; // Update the target to the new one
            isTargetSet = true; // Mark the target as set
            ResetObstructedObjectsVisibility(); // Optionally reset visibility for all previously obstructed objects
        }
    }

    public void ClearTarget()
    {
        target = null; // Clear the current target
        isTargetSet = false; // Reset the target set flag
        ResetObstructedObjectsVisibility(); // Reset visibility
    }

    void OnDisable()
    {
        // Make sure to reset visibility and the target set flag when the camera/script is disabled
        ResetObstructedObjectsVisibility();
        isTargetSet = false;
    }

    private void HideRenderersRecursively(Transform objTransform)
    {
        foreach (Renderer renderer in objTransform.GetComponentsInChildren<Renderer>())
        {
            if (renderer != null && !previouslyObstructed.Contains(renderer))
            {
                renderer.enabled = false; // Hide obstructing object
                previouslyObstructed.Add(renderer); // Add to tracked objects
            }
        }
    }
}
