using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public ScannerMovement scanner;

    float xRotation;
    float yRotation;

    public float cameraScannerModeAngle = 65f;
    public float cameraRotationSpeed = 20f;

    void Start()
    {
        //lock mouse in center of screen and invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (scanner != null)
        {
            // Allow mouse to move camera when not in scanner mode
            if(!scanner.getScannerMode()){
                //get mouse input
                float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
                float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

                yRotation += mouseX;
                xRotation -= mouseY;

                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            }
            // Don't allow camera movement when in scanner mode
            else{
                // Angle camera to look at scanner
                StartCoroutine(RotateObject());
            }
        }
        else
        {
            Debug.LogWarning("Scanner reference is null. Make sure to assign it in the inspector.");
        }
    }

    IEnumerator RotateObject()
    {
        // Calculate the target rotation as a Quaternion
        Quaternion targetRotation = Quaternion.Euler(cameraScannerModeAngle, yRotation, 0);

        // Loop until the object's rotation matches the target rotation
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            // Calculate the rotation step based on the rotation speed and deltaTime
            float step = cameraRotationSpeed * Time.deltaTime;

            // Rotate the object towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

            // Wait for the end of the frame
            yield return null;
        }
    }
}
