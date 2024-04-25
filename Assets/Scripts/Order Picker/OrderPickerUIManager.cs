using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderPickerUIManager : MonoBehaviour
{
    public GameObject endGameModal;
    public GameObject packageCountText;
    public GameObject gameCompletedText;
    public GameObject completedInText;
    public GameObject timerText;
    public GameObject deliveryStatusText;

    public float remainingTime = 300f;
    public float delayBeforeDisabling = 3f;

    public string successText = "Correct Package Delivered!";
    public string gameSuccessText = "Success!";
    public string failureText = "Incorrect Package Delivered!";
    public string gameFailureText = "Game Over!";
    public string packagesDeliveredText = "Packages Delivered:";

    private OrderPickerGameManager orderPickerGameManager;
    private int delivered;
    private int total;
    private bool timerActive = true; // Control flag for the timer

    void Start()
    {
        endGameModal.SetActive(false);
        Cursor.visible = false; // Hide cursor initially
        orderPickerGameManager = GameObject.Find("OrderPickerGameManager").GetComponent<OrderPickerGameManager>();
        deliveryStatusText.SetActive(false);
        UpdatePackageCountText(0, orderPickerGameManager.numPackagesToPick);
        UpdateTimerText(remainingTime);
    }

    void Update()
    {
        if (remainingTime > 0 && timerActive)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerText(remainingTime);
            if (remainingTime <= 0)
            {
                TimerEnded();
            }
        }
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
        delivered = packagesDelivered;
        total = totalPackages;
        packageCountText.GetComponentInChildren<TextMeshProUGUI>().text =
            $"{packagesDeliveredText} ({packagesDelivered}/{totalPackages})";
        if (packagesDelivered.Equals(totalPackages))
        {
            ShowEndScreen(true);
        }
    }

    private IEnumerator HideText(GameObject text)
    {
        yield return new WaitForSeconds(delayBeforeDisabling);
        text.SetActive(false);
    }

    private void ShowEndScreen(bool completedSuccessfuly)
    {
        endGameModal.SetActive(true);
        Cursor.visible = true; // Show cursor in popup view
        timerActive = false; // Stop the timer when showing the end screen
        UpdateCaptions(); // Ensure captions are updated at game end
        gameCompletedText.GetComponentInChildren<TextMeshProUGUI>().text = completedSuccessfuly ? gameSuccessText : gameFailureText;
        completedInText.GetComponentInChildren<TextMeshProUGUI>().text = completedSuccessfuly ?
            $"{delivered}/{total} packages delivered in {FormatElapsedTime()}!" :
            $"Timer ran out with {total - delivered} packages remaining!";
    }

    public void UpdateTimerText(float timeInSeconds)
    {
        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds % 60;
        timerText.GetComponentInChildren<TextMeshProUGUI>().text = $"{minutes:0}:{seconds:00}";
    }

    private void TimerEnded()
    {
        ShowEndScreen(delivered == total);
    }

    private void UpdateCaptions()
    {
        // This method now ensures that the captions are always updated to reflect current game state
        gameCompletedText.GetComponentInChildren<TextMeshProUGUI>().text = (delivered == total) ? gameSuccessText : gameFailureText;
        completedInText.GetComponentInChildren<TextMeshProUGUI>().text = (delivered == total) ?
            $"{delivered}/{total} packages delivered in {FormatElapsedTime()}!" :
            $"Timer ran out with {total - delivered} packages remaining!";
    }

    private string FormatElapsedTime()
    {
        float elapsedTime = 300f - remainingTime; // Assuming the original timer was set to 5 minutes (300 seconds)
        int minutes = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;
        return $"{minutes:0}:{seconds:00}";
    }
}
