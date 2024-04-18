using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorPackageDetector : MonoBehaviour
{
    public Material redColor;
    public Material greenColor;
    public Material greyColor;

    private OrderPickerGameManager orderPickerGameManager;
    public GameObject conveyorBeltBorder;

    private void Start()
    {
        GameObject gameManagerObject = GameObject.Find("OrderPickerGameManager");
        orderPickerGameManager = gameManagerObject.GetComponent<OrderPickerGameManager>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        // Remove package after a delay
        if (collider.GetComponentInParent<Package>() is Package package)
        {
            List<string> targetUUIDs = orderPickerGameManager.GetTargetUUIDs();
            if (targetUUIDs.Contains(package.packageUUID))
            {
                Debug.Log("Correct package delivered!");
                conveyorBeltBorder.GetComponent<Renderer>().material = greenColor;
            }
            else
            {
                Debug.Log("Incorrect package delivered!");
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
