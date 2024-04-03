using UnityEngine;

public class BinCameraManager : MonoBehaviour
{
    public float heightAboveBin = 1.5f; // Height above the bin to place the camera
    public Camera binCamera; // Assign this via the inspector or find it dynamically if there's only one
    public LayerMask obstructingLayer; // Set this to your 'BinViewObstructions' layer

    private void Awake()
    {
        // Optionally, find the BinCamera by tag if not assigned
        if (!binCamera)
            binCamera = GameObject.FindGameObjectWithTag("BinCamera").GetComponent<Camera>();
    }

    private void OnMouseDown()
    {
        // Move the camera over the bin and look at it
        MeshCollider collider = GetComponent<MeshCollider>();
        if (collider != null)
        {
            Vector3 binCenter = collider.bounds.center;
            binCamera.transform.position = new Vector3(binCenter.x, binCenter.y + heightAboveBin, binCenter.z);
            binCamera.transform.LookAt(binCenter);

            // Activate the bin camera and disable the main camera
            if (Camera.main != binCamera)
            {
                Camera.main.gameObject.SetActive(false);
                binCamera.gameObject.SetActive(true);

                // Update the target for visibility management to the current bin
                VisibilityManager visibilityManager = binCamera.GetComponent<VisibilityManager>();
                if (visibilityManager != null)
                {
                    visibilityManager.SetTarget(this.transform);
                }
            }
        }
        else
        {
            Debug.LogError("No MeshCollider component found on the GameObject.");
        }
    }
}
