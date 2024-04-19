using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderPickerUIManager : MonoBehaviour
{
    public GameObject packageCountText;
    public GameObject deliveryStatusText;
    public float delayBeforeDisabling = 1.5f;
    public string successText = "Correct Package Delivered!";
    public string failureText = "Incorrect Package Delivered!";
    public string packagesDeliveredText = "Packages Delivered:";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayFailureMessage()
    {
        deliveryStatusText.GetComponentInChildren<TextMeshProUGUI>().text = failureText;
        deliveryStatusText.SetActive(true);
        StartCoroutine(HideText(deliveryStatusText));
    }

    public void DisplaySuccessMessage()
    {
        deliveryStatusText.GetComponentInChildren<TextMeshProUGUI>().text = successText;
        deliveryStatusText.SetActive(true);
        StartCoroutine(HideText(deliveryStatusText));
    }

    public void UpdatePackageCountText(int packagesDelivered, int totalPackages)
    {
        deliveryStatusText.GetComponentInChildren<TextMeshProUGUI>().text =
            $"{packagesDeliveredText} ({packagesDelivered}/{totalPackages})";
    }

    private IEnumerator HideText(GameObject text)
    {
        yield return new WaitForSeconds(delayBeforeDisabling);
        text.SetActive(false);
    }
}
