using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerScanner scanner;
    public GameObject barcodeScanner;

    private bool isScanner;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        isScanner = false;

        onFoot = playerInput.OnFoot;

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.ToggleScanner.performed += ctx => scanner.Toggle();
        
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        scanner = barcodeScanner.GetComponent<PlayerScanner>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isScanner = scanner.isScannerMode;
        //tell the playermotor to move using the value from our movement action
        if (!isScanner)
            motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate(){
        if (!isScanner)
            look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable(){
        onFoot.Enable();
    }

    private void OnDisable(){
        onFoot.Disable();
    }
}
