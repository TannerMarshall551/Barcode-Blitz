using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    public ScannerMovement scanner;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private SmoothRotation smoothRotation;
    private bool rotationModeActive;

    private void Awake()
    {
        smoothRotation = GetComponent<SmoothRotation>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded && !rotationModeActive)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
<<<<<<< Updated upstream
=======

        // Sprinting input
        if(Input.GetKey(sprintKey) && !isCrouching) // Prevent sprinting while crouching
        {
            moveSpeed = originalMoveSpeed * sprintSpeedMultiplier;
        }
        else if (!isCrouching) // Reset to original speed if not crouching
        {
            moveSpeed = originalMoveSpeed;
        }

        // Crouching input handled in ToggleCrouch()
        if(Input.GetKeyDown(crouchKey) && !rotationModeActive)
        {
            ToggleCrouch();
        }
            
>>>>>>> Stashed changes
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        transform.rotation = orientation.rotation;

        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    
    void Update()
    {
        rotationModeActive = smoothRotation.rotationModeActive;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);

        if (scanner != null)
        {
            if(!scanner.getScannerMode()){
                MyInput();
            }
        }
        else
        {
            Debug.LogWarning("Scanner reference is null. Make sure to assign it in the inspector.");
        }

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if (scanner != null)
        {
            if(!scanner.getScannerMode() && !rotationModeActive){
                MovePlayer();
            }
        }
    }
}
