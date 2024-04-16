using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RPGameState
{
    Tutorial, // Tutorial
    PickUpBox, //Waiting for box to be picked up
    ScanBox, // Scan the label on the box
    PutBoxDown, // Click Dropzone to put down box
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
    public List<string> toyCarStrings;
    private string randomToyString;

    public int chanceCorrect = 80;

    private int score = 0;
    public int scoreToWin = 5;

    private int itemsToTrash;

    private int randBinIndex;

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

        boxManager.SpawnBox();
    }

    // Update is called once per frame
    void Update()
    {
        newBoxUUID = boxManager.GetNewBoxUUID();
        

        switch (currentState)
        {
            case RPGameState.Tutorial:
                if (TutorialComplete())
                {
                    Debug.Log("Tutorial Completed");
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
                    boxManager.DzSetLockDropGrab(true, false);
                    currentState = RPGameState.PutBoxDown;

                    int randomToyIndex = Random.Range(0, 6);

                    randomToy = toyCars[randomToyIndex];

                    float isCorrectNumber = Random.Range(0, 100);


                    if (isCorrectNumber <= 80)
                    {
                        isItemCorrect = true;
                        randomToyString = toyCarStrings[randomToyIndex];
                        randBinIndex = Random.Range(0, 8);
                        correctBin = binList[randBinIndex];
                        correctBinLocation = binLocationList[randBinIndex];
                        boxManager.ToggleBinDZ(randBinIndex, false);
                    }
                    else
                    {
                        isItemCorrect = false;

                        int randomStringIndex = Random.Range(0, 6);
                        while(randomStringIndex == randomToyIndex)
                        {
                            randomStringIndex = Random.Range(0, 6);
                        }
                        randomToyString = toyCarStrings[randomStringIndex];
                    }

                    
                    

                    //UpdateScannerInterface();
                }
                break;
            case RPGameState.PutBoxDown:
                if (PutBoxDownComplete())
                {
                    Debug.Log("PutBoxDown Completed");
                    boxManager.OpenBox(randomToy, randBinIndex);
                    boxManager.DzSetLockDropGrab(false, true);
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
                        currentState = RPGameState.ScanShelf;
                    }
                    else
                    {
                        Debug.Log("Incorrect item, trash it");
                        boxManager.SetTrashLockDrop(false);
                        itemsToTrash = 2;
                        currentState = RPGameState.TrashItem;
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
                    currentState = RPGameState.TrashItem;
                }
                break;
            case RPGameState.TrashItem:
                if(TrashItemComplete(itemsToTrash))
                {
                    boxManager.SetTrashLockDrop(true);
                    score++;
                    if(score >= scoreToWin)
                        currentState = RPGameState.End;
                    else
                    {
                        boxManager.SpawnBox();
                        currentState = RPGameState.PickUpBox;
                    }
                    Debug.Log("Score: " + score);
                }
                break;
            case RPGameState.End:
                break;
            default:
                Debug.LogWarning("Unknown RPGameState");
                break;
        }
    }

    public bool StateComplete()
    {
        if (stateComplete)
        {
            stateComplete = false;
            return true;
        }
        return false;
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

    private bool MarkIfCorrectComplete()
    {
        return true;
    }

    private bool ScanShelfComplete()
    {
        if (lastScannedUUID != null && lastScannedUUID.Equals(correctBin))
        {
            lastScannedUUID = null;
            return true;
        }
        else
        {
            lastScannedUUID = null;
            return false;
        }
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
}
