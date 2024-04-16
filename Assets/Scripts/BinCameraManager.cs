using UnityEngine;

public class BinCameraManager : MonoBehaviour
{
    public float heightAboveBin = 2f;
    public Camera binCamera;
    public Camera mainCamera;
    public LayerMask obstructingLayer; // Any potential obstructions to the bins (i.e. shelves) should be added here

    private PlayerPickupDrop playerPickupDrop;

    private void Awake()
    {
        if (!binCamera)
        {
            binCamera = GameObject.FindGameObjectWithTag("BinCamera").GetComponent<Camera>();
        }
        if (!mainCamera)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        GameObject playerObject = GameObject.Find("Player");
        playerPickupDrop = playerObject.GetComponent<PlayerPickupDrop>();
    }

    private void OnMouseUp()
    {
        if (!playerPickupDrop.isHolding)
        {
            // Move camera over bin, look down
            MeshCollider collider = GetComponent<MeshCollider>();
            VisibilityManager visibilityManager = binCamera.GetComponent<VisibilityManager>();
            if (collider != null)
            {
                if (!visibilityManager.isTargetSet)
                {
                    Vector3 binCenter = collider.bounds.center;
                    binCamera.transform.position = new Vector3(binCenter.x, binCenter.y + heightAboveBin, binCenter.z);
                    binCamera.transform.LookAt(binCenter);
                }

                // Activate bin camera, disable main camera
                if (!binCamera.isActiveAndEnabled)
                {
                    //VisibilityManager visibilityManager = binCamera.GetComponent<VisibilityManager>();
                    if (visibilityManager != null)
                    {
                        visibilityManager.SetTarget(this.transform);
                    }
                    mainCamera.gameObject.SetActive(false);
                    binCamera.gameObject.SetActive(true);
                    binCamera.enabled = true;
                }
            }
            else
            {
                Debug.LogError("No MeshCollider component found on the GameObject.");
            }
        }
    }
}
