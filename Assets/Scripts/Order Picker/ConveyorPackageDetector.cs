using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorPackageDetector : MonoBehaviour
{
    public Material redColor;
    public Material greenColor;
    public Material greyColor;

    private OrderPickerGameManager orderPickerGameManager;
    private OrderPickerUIManager orderPickerUIManager;
    public GameObject conveyorBeltBorder;
    private List<string> pastDeliveries = new();

    private void Start()
    {
        GameObject gameManagerObject = GameObject.Find("OrderPickerGameManager");
        orderPickerGameManager = gameManagerObject.GetComponent<OrderPickerGameManager>();
        orderPickerUIManager = GameObject.Find("OrderPickerUIOverlay").GetComponent<OrderPickerUIManager>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        // Remove package after a delay
        if (collider.GetComponentInParent<Package>() is Package package)
        {
            List<string> targetUUIDs = orderPickerGameManager.GetTargetUUIDs();
            if (targetUUIDs.Contains(package.packageUUID))
            {
                pastDeliveries.Add(package.packageUUID);
                //Debug.Log("Correct package delivered!");
                int packagesRemaining = orderPickerGameManager.AcknowledgeSuccessfulDelivery(package.packageUUID);
                orderPickerUIManager.DisplaySuccessMessage();
                orderPickerUIManager.UpdatePackageCountText(packagesRemaining, orderPickerGameManager.numPackagesToPick);
                conveyorBeltBorder.GetComponent<Renderer>().material = greenColor;
            }
            else if (!pastDeliveries.Contains(package.packageUUID))
            {
                //Debug.Log("Incorrect package delivered!");
                orderPickerUIManager.DisplayFailureMessage();
                conveyorBeltBorder.GetComponent<Renderer>().material = redColor;
            }
            StartCoroutine(RemovePackageAfterDelay(collider.gameObject));
        }
    }

    IEnumerator RemovePackageAfterDelay(GameObject package)
    {
        yield return new WaitForSeconds(1);
        Destroy(package);
        conveyorBeltBorder.GetComponent<Renderer>().material = greyColor;
    }
}
