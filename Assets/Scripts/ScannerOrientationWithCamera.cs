using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerOrientationWithCamera : MonoBehaviour
{
    public GameObject cameraContainer;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    void Update()
    {
        // Ensure the camera reference is not null
        if (cameraContainer != null)
        {
            // Calculate the desired position and rotation relative to the camera
            Vector3 desiredPosition = cameraContainer.transform.position + positionOffset;
            Quaternion desiredRotation = cameraContainer.transform.rotation * Quaternion.Euler(rotationOffset);

            // Set the object's position and rotation
            transform.position = desiredPosition;
            transform.rotation = desiredRotation;
        }
        else
        {
            Debug.LogWarning("Main camera reference is null. Make sure to assign the camera in the inspector.");
        }
    }
}
