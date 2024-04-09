using UnityEngine;

public class BinCameraManager : MonoBehaviour
{
    public float heightAboveBin = 2f;
    public Camera binCamera;
    public Camera mainCamera;
    public LayerMask obstructingLayer; // Any potential obstructions to the bins (i.e. shelves) should be added here

    private void Awake()
    {
        // Find BinCamera by tag
        if (!binCamera)
        {
            binCamera = GameObject.FindGameObjectWithTag("BinCamera").GetComponent<Camera>();
        }
        if (!mainCamera)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    private void OnMouseDown()
    {
        // Move camera over bin, look down
        MeshCollider collider = GetComponent<MeshCollider>();
        if (collider != null)
        {
            Vector3 binCenter = collider.bounds.center;
            binCamera.transform.position = new Vector3(binCenter.x, binCenter.y + heightAboveBin, binCenter.z);
            binCamera.transform.LookAt(binCenter);

            // Activate bin camera, disable main camera
            if (Camera.main != binCamera)
            {
                mainCamera.gameObject.SetActive(false);
                binCamera.gameObject.SetActive(true);

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
