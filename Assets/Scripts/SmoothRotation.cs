using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    private PlayerPickupDrop playerPickupDrop;
    public bool isHolding;
    public bool rotationModeActive;

    private GameObject objectToRotate;

    Quaternion targetRotation;

    private void Awake()
    {
        playerPickupDrop = GetComponent<PlayerPickupDrop>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        objectToRotate = GetComponent<PlayerPickupDrop>().boxBeingHeld;

        isHolding = playerPickupDrop.isHolding;
        if (Input.GetKeyDown(KeyCode.R) && isHolding)
        {
            if (rotationModeActive)
            {
                rotationModeActive = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                rotationModeActive = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                objectToRotate.transform.rotation = Quaternion.Euler(0f, objectToRotate.transform.eulerAngles.y, 0f);
            }
        }

        if (!isHolding)
        {
            rotationModeActive = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (rotationModeActive)
        {
             if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
             {
                 RotateUp();
             }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RotateLeft();
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                RotateDown();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                RotateRight();
            }
        }
    }

    private void RotateLeft()
    {
        targetRotation = Quaternion.Euler(objectToRotate.transform.eulerAngles.x, objectToRotate.transform.eulerAngles.y + 90, objectToRotate.transform.eulerAngles.z);

        objectToRotate.transform.rotation = targetRotation;
    }

    private void RotateRight()
    {
        targetRotation = Quaternion.Euler(objectToRotate.transform.eulerAngles.x, objectToRotate.transform.eulerAngles.y - 90, objectToRotate.transform.eulerAngles.z);

        objectToRotate.transform.rotation = targetRotation;
    }

    private void RotateUp()
    {
    targetRotation = Quaternion.Euler(objectToRotate.transform.eulerAngles.x + 90, objectToRotate.transform.eulerAngles.y, 0f);

    objectToRotate.transform.rotation = targetRotation;
    }

    private void RotateDown()
    {
    targetRotation = Quaternion.Euler(objectToRotate.transform.eulerAngles.x - 90, objectToRotate.transform.eulerAngles.y, 0f);

    objectToRotate.transform.rotation = targetRotation;
    }
}