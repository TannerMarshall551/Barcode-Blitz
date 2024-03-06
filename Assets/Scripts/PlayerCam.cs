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

    public float cameraScannerModeAngle = 45f;
    public float cameraRotationSpeed = 20f;

    public bool showMouse = false;
    public bool lockMouse = true;

    [SerializeField] GameObject player;
    private SmoothRotation smoothRotation;
    private bool rotationModeActive;

    private void Awake()
    {
        smoothRotation = player.GetComponent<SmoothRotation>();
    }

    void Start()
    {
        //lock mouse in center of screen and invisible
        Cursor.lockState = lockMouse ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = showMouse;
    }

    // Update is called once per frame
    void Update()
    {
        rotationModeActive = smoothRotation.rotationModeActive;

        if (scanner != null)
        {
            // Allow mouse to move camera when not in scanner mode
            if(!scanner.getScannerMode() && !rotationModeActive)
            {
                //get mouse input
                float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
                float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

                yRotation += mouseX;
                xRotation -= mouseY;

                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                orientation.rotation = Quaternion.Euler(0, yRotation, 0);

                showMouse = false;
                lockMouse = true;
            }
            // Don't allow camera movement when in scanner mode
            else if (!rotationModeActive)
            {
                // Angle camera to look at scanner
                StartCoroutine(RotateObject());
                
                showMouse = true;
                lockMouse = false;
            }
        }
        else
        {
            Debug.LogWarning("Scanner reference is null. Make sure to assign it in the inspector.");
        }

        // Cursor.lockState = lockMouse ? CursorLockMode.Locked : CursorLockMode.None;
        // Cursor.visible = showMouse;
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
