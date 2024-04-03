using UnityEngine;

public class MoveCamOverBin : MonoBehaviour
{
    public float heightAboveBin = 1.5f;

    private void OnMouseDown()
    {
        GameObject cameraGameObject = GameObject.FindGameObjectWithTag("BinCamera");
        if (cameraGameObject != null)
        {
            Camera camera = cameraGameObject.GetComponent<Camera>();
            if (camera != null)
            {
                MeshCollider collider = GetComponent<MeshCollider>();
                if (collider != null)
                {
                    Vector3 binCenter = collider.bounds.center;
                    Vector3 newPosition = new Vector3(binCenter.x, binCenter.y + heightAboveBin, binCenter.z);

                    camera.transform.position = newPosition;
                    camera.transform.LookAt(binCenter);

                    if (Camera.main != camera)
                    {
                        Camera.main.gameObject.SetActive(false);
                        camera.gameObject.SetActive(true); // Activate bin camera
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