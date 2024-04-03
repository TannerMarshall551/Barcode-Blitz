using UnityEngine;

public class MoveCamOverBin : MonoBehaviour
{
    public float heightAboveBin = 10f; // Height above the bin to place the camera

    private void OnMouseDown()
    {
        Debug.Log("Clicked bin!");
        // Find the camera by tag
        GameObject cameraGameObject = GameObject.FindGameObjectWithTag("BinCamera");
        if (cameraGameObject != null)
        {
            Debug.Log("Found camera game object!");
            Camera camera = cameraGameObject.GetComponent<Camera>();
            if (camera != null)
            {
                MeshCollider collider = GetComponent<MeshCollider>();
                if (collider != null)
                {
                    Vector3 binCenter = collider.bounds.center;
                    Vector3 newPosition = new Vector3(binCenter.x, binCenter.y + heightAboveBin, binCenter.z);

                    // Move the camera to the new position
                    camera.transform.position = newPosition;

                    // Point the camera directly down towards the bin's center
                    camera.transform.LookAt(binCenter);

                    // Ensure this camera is the active one
                    if (Camera.main != camera) // Check if the current main camera is different from this camera
                    {
                        Camera.main.gameObject.SetActive(false); // Disable the current main camera
                        camera.gameObject.SetActive(true); // Activate this camera
                    }
                }
                else
                {
                    Debug.LogError("No MeshCollider component found on the GameObject.");
                }
            }
            else
            {
                Debug.LogError("No Camera component found on the GameObject tagged as 'BinCamera'.");
            }
        }
        else
        {
            Debug.LogError("No GameObject found with the tag 'BinCamera'.");
        }
    }
}