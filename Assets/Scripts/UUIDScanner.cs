//using System.Diagnostics;
using UnityEngine;

/*
 * Checks whether mouse is over Package prefab's label while clicking;
 *  If it is, log UUID of the Package prefab's label
 */

public class UUIDScanner : MonoBehaviour
{

    public ReturnsProcessorGameManager rpGameManager;
    public OrderPackerGameManager opGameManager;
    public OrderPickerGameManager orderPickerGameManager;

    void Start(){
        GameObject opGM = GameObject.Find("GameManager");
        if(opGM != null){
            opGameManager = opGM.GetComponent<OrderPackerGameManager>();
        }
        GameObject orderPickerGameManagerObject = GameObject.Find("OrderPickerGameManager");
        if(orderPickerGameManagerObject != null)
        {
            orderPickerGameManager = orderPickerGameManagerObject.GetComponent<OrderPickerGameManager>();
        }
    }

    void Update()
    {
        // Check left mouse button clicked
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Alpha1)) // 1 is number for right mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the ray hits the collider component of plane (label)
                //if (hit.collider.gameObject == gameObject)
                //{
                    // Log UUID
                    // Check UUID stored in component attached to parent GameObject
                    UUIDGenerator uuidGenerator = hit.collider.GetComponentInParent<UUIDGenerator>();
                    if (uuidGenerator != null)
                    {
                    string uuid = uuidGenerator.GetUUID();
                        Debug.Log("UUID: " + uuid);
                        if(orderPickerGameManager != null)
                        {
                            orderPickerGameManager.ScanPackage(uuid);
                        }
                        if(rpGameManager != null)
                        {
                            rpGameManager.SetLastScannedUUID(uuid);
                        }
                        if(opGameManager != null)
                        {
                            opGameManager.ScanItem(uuid);
                        }
                    }
                //}
            }
        }
    }
}
