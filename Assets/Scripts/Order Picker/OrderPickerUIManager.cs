using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderPickerUIManager : MonoBehaviour
{
    public GameObject packageCountText;
    public GameObject deliveryStatusText;
    public float delayBeforeDisabling = 3f;
    public string successText = "Correct Package Delivered!";
    public string failureText = "Incorrect Package Delivered!";
    public string packagesDeliveredText = "Packages Delivered:";
    private OrderPickerGameManager orderPickerGameManager;

    // Start is called before the first frame update
    void Start()
    {
        orderPickerGameManager = GameObject.Find("OrderPickerGameManager").GetComponent<OrderPickerGameManager>();
        deliveryStatusText.SetActive(false);
        UpdatePackageCountText(0, orderPickerGameManager.numPackagesToPick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayFailureMessage()
    {
        TextMeshProUGUI textMeshProUGUI = deliveryStatusText.GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.text = failureText;
        textMeshProUGUI.color = new Color32(255, 0, 11, 255);
        deliveryStatusText.SetActive(true);
        StartCoroutine(HideText(deliveryStatusText));
    }

    public void DisplaySuccessMessage()
    {
        TextMeshProUGUI textMeshProUGUI = deliveryStatusText.GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.text = successText;
        textMeshProUGUI.color = new Color32(0, 217, 27, 255);
        deliveryStatusText.SetActive(true);
        StartCoroutine(HideText(deliveryStatusText));
    }

    public void UpdatePackageCountText(int packagesDelivered, int totalPackages)
    {
        packageCountText.GetComponentInChildren<TextMeshProUGUI>().text =
            $"{packagesDeliveredText} ({packagesDelivered}/{totalPackages})";
    }

    private IEnumerator HideText(GameObject text)
    {
        yield return new WaitForSeconds(delayBeforeDisabling);
        text.SetActive(false);
    }
}
