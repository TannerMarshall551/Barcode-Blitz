using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RPGameState
{
    Tutorial, // Tutorial
    PickUpBox, //Waiting for box to be picked up
    ScanBox, // Scan the label on the box
    PutBoxDown, // Click Dropzone to put down box
    PutItemDown, //Take item out of box and put into smaller drop zone
    MarkIfCorrect, // Mark on scanner if items are correct
    ScanShelf, // Scan label on shelf to put item away
    PlaceItem, // Put the item away
    TrashItem, //If item is wrong throw in trash
    End // Prompt to retry 
}

public class ReturnsProcessorGameManager : MonoBehaviour
{
    private RPGameState currentState; // current state the game is in

    // other managers
    private ReturnsProcessorBoxManager boxManager;
    private ReturnsProcessorScannerManager scannerManager;

    private bool stateComplete = false; // if the current state is complete

    private bool isItemCorrect;

    private string newBoxUUID;

    private string lastScannedUUID;

    private List<string> binList = new List<string> { "1a", "1b", "1c", "1d", "2a", "2b", "2c", "2d" };
    private List<string> binLocationList = new List<string> { "Row 1, Column 1", "Row 1, Column 2", "Row 1, Column 3", "Row 1, Column 4",
        "Row 2, Column 1", "Row 2, Column 2", "Row 2, Column 3", "Row 2, Column 4" };
    private string correctBinLocation;

    private string correctBin;

    public List<GameObject> toyCars;
    private GameObject randomToy;
    private List<string> toyCarStrings = new List<string> { "Blue Car", "Orange Car", "Yellow Car" };
    private string randomToyString;

    public int chanceCorrect = 80;

    private int score = 0;
    public int scoreToWin = 5;

    private int itemsToTrash;

    private int randBinIndex;

    public Timer timer;
    private bool gameOver = false;

    private bool scanCorrect = false;

    // Start is called before the first frame update
    void Start()
    {
        currentState = RPGameState.Tutorial;

        // Get the item manager, box manager, and scanner manager in scene
        GameObject boxManagerObj = GameObject.Find("BoxManager");
        if (boxManagerObj == null)
            Debug.LogError("No BoxManager Object");

        boxManager = boxManagerObj.GetComponent<ReturnsProcessorBoxManager>();

        if (boxManager == null)
            Debug.LogError("No BoxManager Loaded");

        GameObject scannerManagerObj = GameObject.Find("ScannerManager");
        if (scannerManagerObj == null)
            Debug.LogError("No ScannerManager Object");

        scannerManager = scannerManagerObj.GetComponent<ReturnsProcessorScannerManager>();

        if (boxManager == null)
            Debug.LogError("No ScannerManager Loaded");

        boxManager.SpawnBox();
    }

    // Update is called once per frame
    void Update()
    {
        newBoxUUID = boxManager.GetNewBoxUUID();
        if(timer.GetGameOver() && !gameOver)
        {
            currentState = RPGameState.End;
            scannerManager.YouLosePage();
            gameOver = true;
        }

        switch (currentState)
        {
            case RPGameState.Tutorial:
                if (TutorialComplete())
                {
                    Debug.Log("Tutorial Completed");
                    scannerManager.StartPage();
                    currentState = RPGameState.PickUpBox;
                }
                break;
            case RPGameState.PickUpBox:
                if (PickUpBoxComplete())
                {
                    Debug.Log("PickUpBox Completed");
                    currentState = RPGameState.ScanBox;
                }
                break;
            case RPGameState.ScanBox:
                if (ScanBoxCompleted())
                {
                    Debug.Log("ScanBox Completed");
                    boxManager.DzSetLockDropGrab(false, true);
                    currentState = RPGameState.PutBoxDown;

                    int randomToyIndex = Random.Range(0, toyCarStrings.Count);

                    randomToy = toyCars[randomToyIndex];

                    float isCorrectNumber = Random.Range(0, 100);


                    if (isCorrectNumber <= 80)
                    {
                        isItemCorrect = true;
                        randomToyString = toyCarStrings[randomToyIndex];
                        randBinIndex = Random.Range(0, 8);
                        boxManager.RemoveItemFromBin(randBinIndex);
                        correctBin = binList[randBinIndex];
                        correctBinLocation = binLocationList[randBinIndex];
                        boxManager.ToggleBinDZ(randBinIndex, false);
                    }
                    else
                    {
                        isItemCorrect = false;

                        int randomStringIndex = Random.Range(0, toyCarStrings.Count);
                        while(randomStringIndex == randomToyIndex)
                        {
                            randomStringIndex = Random.Range(0, toyCarStrings.Count);
                        }
                        randomToyString = toyCarStrings[randomStringIndex];
                    }

                    scannerManager.MarkIfCorrectPage();
                }
                break;
            case RPGameState.PutBoxDown:
                if (PutBoxDownComplete())
                {
                    lastScannedUUID = null;
                    Debug.Log("PutBoxDown Completed");
                    boxManager.OpenBox(randomToy, randBinIndex);
                    boxManager.DzSetLockDropGrab(false, false);
                    boxManager.ToggleItemDZ(false, false);
                    currentState = RPGameState.PutItemDown;
                }
                break;
            case RPGameState.PutItemDown:
                if(PutItemDownComplete())
                {
                    Debug.Log("PutItemDown Complete");
                    boxManager.DzSetLockDropGrab(true, false);
                    currentState = RPGameState.MarkIfCorrect;
                }
                break;
            case RPGameState.MarkIfCorrect:
                if (MarkIfCorrectComplete())
                {
                    Debug.Log("Mark If Correct Completed");
                    if (isItemCorrect)
                    {
                        Debug.Log("Correct Bin: " + correctBin);
                        itemsToTrash = 1;
                        scannerManager.CorrectItemPage();
                        currentState = RPGameState.ScanShelf;
                    }
                    else
                    {
                        Debug.Log("Incorrect item, trash it");
                        boxManager.SetTrashLockDrop(false);
                        boxManager.ReenableColliders();
                        itemsToTrash = 2;
                        currentState = RPGameState.TrashItem;
                        scannerManager.Discard();
                    }
                }
                break;
            case RPGameState.ScanShelf:
                if (ScanShelfComplete())
                {
                    Debug.Log("ScanShelfComplete");
                    currentState = RPGameState.PlaceItem;
                }
                break;
            case RPGameState.PlaceItem:
                if(PlaceItemComplete())
                {
                    Debug.Log("PlaceItem Complete");
                    boxManager.SetTrashLockDrop(false);
                    boxManager.ReenableColliders();
                    boxManager.ToggleItemDZ(true, true);
                    currentState = RPGameState.TrashItem;
                    scannerManager.DiscardBoxPage();
                }
                break;
            case RPGameState.TrashItem:
                if(TrashItemComplete(itemsToTrash))
                {
                    boxManager.SetTrashLockDrop(true);
                    score++;
                    if(score >= scoreToWin)
                    {
                        scannerManager.YouWinPage();
                        currentState = RPGameState.End;
                    }  
                    else
                    {
                        boxManager.SpawnBox();
                        currentState = RPGameState.PickUpBox;
                        scannerManager.StartPage();
                    }
                    Debug.Log("Score: " + score);
                }
                break;
            case RPGameState.End:
                gameOver = true;
                break;
            default:
                Debug.LogWarning("Unknown RPGameState");
                break;
        }
    }

    // checks if space is pressed (temporary)
    public bool SpacePressed()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }

    // tutorial completed
    public bool TutorialComplete()
    {
        return SpacePressed(); // TODO
    }

    public bool PickUpBoxComplete()
    {
        return boxManager.HoldingNewBox();
    }

    public bool PutBoxDownComplete()
    {
        return boxManager.IsDZFull();
    }

    public bool ScanBoxCompleted()
    {
        if (lastScannedUUID != null && lastScannedUUID.Equals(newBoxUUID))
        {
            lastScannedUUID = null;
            return true;
        }
        else
            return false;
    }

    public bool PutItemDownComplete()
    {
        return boxManager.CheckItemDZFull();
    }

    private bool MarkIfCorrectComplete()
    {
        if (scanCorrect)
        {
            scanCorrect = false;
            return true;
        }
        else
            return false;
    }

    private bool ScanShelfComplete()
    {
        if (lastScannedUUID != null && lastScannedUUID.Equals(correctBin))
        {
            scannerManager.CorrectItemPageGreen();
            lastScannedUUID = null;
            return true;
        }
        else if (lastScannedUUID != null)
        {
            scannerManager.CorrectItemPageRed();
            lastScannedUUID = null;
            return false;
        }
        else
            return false;
    }

    private bool PlaceItemComplete()
    {
        return boxManager.CheckBinDZFull(randBinIndex);
    }

    private bool TrashItemComplete(int numItemsToTrash)
    {
        return boxManager.AllItemsTrashed(numItemsToTrash);
    }

    public void SetLastScannedUUID(string scan)
    {
        lastScannedUUID = scan;
    }

    public string GetCarTypeForScanner()
    {
        return randomToyString;
    }

    public string GetBinLocationForScanner()
    {
        return correctBinLocation;
    }

    public RPGameState GetCurrentState()
    {
        return currentState;
    }

    public int GetScore()
    {
        return score;
    }

    public void CheckIfCorrectButton(int yesOrNo)
    {
        if((yesOrNo == 0 && isItemCorrect) || (yesOrNo == 1 && !isItemCorrect))
        {
            scanCorrect = true;
        }
        else
        {
            scanCorrect = false;
        }
       
    }

    public bool GetGameOver()
    {
        return gameOver;
    }
}
