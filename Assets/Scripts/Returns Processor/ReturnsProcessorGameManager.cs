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
    private string correctBin;

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
                if(PickUpBoxComplete())
                {
                    Debug.Log("PickUpBox Completed");
                    currentState = RPGameState.ScanBox;
                }
                break;
            case RPGameState.ScanBox:
                if(ScanBoxCompleted())
                {
                    Debug.Log("ScanBox Completed");
                    boxManager.DzSetLockDropGrab(true, false);
                    currentState = RPGameState.PutBoxDown;

                    float isCorrectNumber = boxManager.GetIsCorrect();
                    if (isCorrectNumber <= 80)
                        isItemCorrect = true;
                    else
                        isItemCorrect = false;

                    //UpdateScannerInterface();
                }
                break;
            case RPGameState.PutBoxDown:
                if(PutBoxDownComplete())
                {
                    Debug.Log("PutBoxDown Completed");
                    isItemCorrect = boxManager.OpenBox();
                    boxManager.DzSetLockDropGrab(false, true);
                    currentState = RPGameState.MarkIfCorrect;
                }
                break;
            case RPGameState.MarkIfCorrect:
                if(MarkIfCorrectComplete())
                {
                    Debug.Log("Mark If Correct Completed");
                    if (isItemCorrect)
                    {
                        int randBinIndex = Random.Range(0, 6);
                        correctBin = binList[randBinIndex];
                        Debug.Log("Correct Bin: " + correctBin);
                        currentState = RPGameState.ScanShelf;
                    }
                    else
                    {
                        Debug.Log("Incorrect item, trash it");
                        currentState = RPGameState.TrashItem;
                    }
                }
                break;
            case RPGameState.ScanShelf:
                if(ScanShelfComplete())
                {
                    Debug.Log("ScanShelfComplete");
                }
                break;
            case RPGameState.PlaceItem:
                break;
            case RPGameState.TrashItem:
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

    public void SetLastScannedUUID(string scan)
    {
        lastScannedUUID = scan;
    }

    private bool MarkIfCorrectComplete()
    {
        return true;
    }

    private bool ScanShelfComplete()
    {
        if(lastScannedUUID != null && lastScannedUUID.Equals(correctBin))
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
}
