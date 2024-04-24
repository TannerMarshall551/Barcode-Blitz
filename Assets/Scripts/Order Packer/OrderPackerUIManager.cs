using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class OrderPackerUIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the Text component
    public TextMeshProUGUI countText; // Reference to the Text component
    public TextMeshProUGUI tutorialText; // Reference to the Tutorial Text
    public TextMeshProUGUI instructionsText;
    public TextBackgroundBubble instructionsTextBackground;
    public GameObject tutorialBubble; // Reference to a UI object that contains the tutorial text

    private float startTime;
    private bool timerActive = false;
    private int currentTutorialIndex = 0;

    private string[] tutorialMessages = {
        "Hello! I'm the manager! I'll be explaining how things work around here.",
        "To move throughout the game use WASD and the cursor to look around!",
        "Time to pack some items!",
        "To start, press E to look at your scanner and press start package.",
        "Head to the table and pick up an unfolded box and place it on the table.",
        "On the shelf behind you you’ll see some items. Press E to check your barcode scanner to see what items you need to pack!",
        "This barcode scanner will tell you what packages you need to pack and how many. Click the arrow to scroll through the pages and see what you need.",
        "When an item is packed fully the page will turn green to indicate you have finished packing that item.",
        "While you are still packing, the page will turn yellow to indicate that there are multiple of the same item that need to be packed.",
        "Once you’ve picked up an item you can inspect the item to check if it's damaged. Press R to rotate and inspect the item using WASD.",
        "If the item is not damaged, scan the item by right clicking it and place the item in the open box you just made. Keep this up until all the items are packed.",
        "If you find any damage on the item, there is a button on the barcode scanner to indicate the item you need to package is damaged. Press that button and place the item in the trash next to the shelf.",
        "Once you are done packing, on the last page of the barcode scanner is a button to indicate you have completed the package.",
        "Now that you're done packing all the items press the complete button on the scanner and grab a packing label next to the open box.",
        "Place the packing label on the open box to close the box. Place the closed box on the conveyor belt and continue on to the next package!",
        "Great! You’ve completed your first package! Now you can do the rest on your own, if you have any questions press I to restart the tutorial. Good luck!"
    };

    // using 0, 1, 2, 4
    private string[] errorMessages = {
        "This item looks like it's damaged and can’t be packed! Press the damaged button on the barcode scanner and throw it in the trash.",
        "It looks like this item isn’t damaged! You can scan and pack this item in the box.",
        "It looks like this item doesn’t match the item currently on the scanner! Find the correct item and scan and pack it.",
        "It looks like you can’t place the item there! Make sure it is the correct item or that you have scanned the item beforehand.",
        "It looks like you've finished packing these items! Use the arrows on the barcode scanner to see what other items need to be packed."
    };

    private string endMessage = "Congradulations! You've packed all the boxes! Great Work!";

    void Start(){
        if(timerText == null){
            Debug.LogWarning("No Timer TMP Loaded");
        }
        if(countText == null){
            Debug.LogWarning("No Counter TMP Loaded");
        }
        if(tutorialText == null){
            Debug.LogWarning("No Tutorial TMP Loaded");
        }
        if(instructionsText == null){
            Debug.LogWarning("No Instructions TMP loaded");
        }
        ShowTutorialText(true);
        GetCurText();
    }

    void Update()
    {
        if (timerText != null && timerActive)
        {
            float t = Time.time - startTime;
            t = Mathf.FloorToInt(t);
            string minutes = ((int) t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;
        }

        if (Input.GetKeyDown(KeyCode.I)) // Check if the space bar is pressed
        {
            ShowTutorialText(true);
            currentTutorialIndex = 0;
            GetCurText();


        }

        if (Input.GetKeyDown(KeyCode.Period)) // Check if the space bar is pressed
        {
            GetNextText();
        }
        if (Input.GetKeyDown(KeyCode.Comma)) // Check if the space bar is pressed
        {
            GetPrevText();
        }
    }

    // Logs the error message
    public void ShowErrorText(int index){
        if (tutorialBubble != null)
        {
            tutorialText.text = errorMessages[index];
            UpdateTutorialBubble();

            tutorialBubble.SetActive(true); // Control the visibility of the tutorial text bubble
        }
    }

    // Logs the error message
    public void ShowEndText(){
        if (tutorialBubble != null)
        {
            tutorialText.text = endMessage;
            UpdateTutorialBubble();

            tutorialBubble.SetActive(true); // Control the visibility of the tutorial text bubble
        }
    }

    // Show or hide tutorial bubble
    public void ShowTutorialText(bool show)
    {
        if (tutorialBubble != null)
        {
            if(show){
                instructionsText.text = "Press < > to navigate tutorial";
            }
            else{
                instructionsText.text = "Press I for Tutorial";
            }

            tutorialBubble.SetActive(show); // Control the visibility of the tutorial text bubble
        }
    }

    public void UpdateTutorialBubble(){
        if(instructionsTextBackground != null)
            instructionsTextBackground.UpdateBubble();
    }

    // Change tutorial text
    public void GetNextText()
    {
        if (tutorialText != null && currentTutorialIndex < tutorialMessages.Length-1)
        {
            tutorialText.text = tutorialMessages[++currentTutorialIndex];
            UpdateTutorialBubble();
        }
        else{
            ShowTutorialText(false);
        }

    }

    // Change tutorial text
    public void GetPrevText()
    {
        if (tutorialText != null && currentTutorialIndex > 0)
        {
            tutorialText.text = tutorialMessages[--currentTutorialIndex];
            UpdateTutorialBubble();
        }
        else{
            ShowTutorialText(false);
        }

    }

    public void GetCurText()
    {
        if (tutorialText != null)
        {
            tutorialText.text = tutorialMessages[currentTutorialIndex];
            UpdateTutorialBubble();
        }
        else{
            ShowTutorialText(false);
        }

    }

    public void StartTimer(){
        timerActive = true;
        startTime = Time.time;
    }

    // Call this to stop the timer
    public void StopTimer()
    {
        timerActive = false;
    }

    public void AddTime(float seconds){
        startTime += seconds;
    }

    //
    public void SetCountText(string countText){
        if(this.countText != null){
            this.countText.text = countText;
        }
    }
}