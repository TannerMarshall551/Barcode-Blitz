using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    Tutorial,
    Start, // Click the scanner to start
    MakeBox, // Make the box
    GrabItems, // Move items to box
    CompleteBox, // Move package to conveyer
    End
}

public class OrderPackerGameManager : MonoBehaviour
{
    private GameState currentState;

    private OrderPackerItemManager itemManager;
    private OrderPackerBoxManager boxManager;

    public ScannerItemManager scannerManager;

    private List<GameObject> currentItems;

    private const int MAXITEMS = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.Tutorial;

        // Get the item manager and box manager in scene
        GameObject boxManagerObj = GameObject.Find("BoxManager");
        GameObject itemManagerObj = GameObject.Find("ItemManager");

        if(boxManagerObj == null){
            Debug.LogError("No BoxManager Object");
        }
        if(itemManagerObj == null){
            Debug.LogError("No ItemManager Object");
        }
        
        boxManager = boxManagerObj.GetComponent<OrderPackerBoxManager>();
        itemManager = itemManagerObj.GetComponent<OrderPackerItemManager>();

        if(boxManager == null){
            Debug.LogError("No BoxManager Loaded");
        }
        if(itemManager == null){
            Debug.LogError("No ItemManager Loaded");
        }
        if(scannerManager == null){
            Debug.LogError("No ScannerManager Loaded");
        }

        itemManager.SpawnObjectsOnShelf();
        boxManager.SpawnBoxes(5);     
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.Tutorial:
                if(TutorialComplete()){
                    Debug.Log("TODO Tutorial");
                    itemManager.LockAllItems();
                    boxManager.SetLockedBox(BoxState.Unmade, true);
                    currentState = GameState.Start;
                }
                break;
            case GameState.Start:
                // if(StartComplete()){
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Start Completed");
                    currentState = GameState.MakeBox;
                    GetNextItems();
                    boxManager.SetLockedBox(BoxState.Unmade, false);
                }
                break;
            case GameState.MakeBox:
                if(MakeBoxComplete()){
                    currentState = GameState.GrabItems;
                }
                break;
            case GameState.GrabItems:
                if(GrabItemsComplete()){
                    currentState = GameState.CompleteBox;
                }
                break;
            case GameState.CompleteBox:
                if(CompleteBoxComplete()){
                    // Go to start unless all boxes completed
                }
                break;
            case GameState.End:
                if(EndComplete()){
                    // Prompt to start again or end
                }
                break;
            default:
                Debug.LogWarning("Unknown GameState");
                break;

        } 
    }

    // tutorial completed
    public bool TutorialComplete(){
        return true;
    }
    // start button pressed
    public bool StartComplete(){
        return true;
    }
    // moved unopen box into next drop zone (opening the box)
    public bool MakeBoxComplete(){
        return false;
    }
    // all items in next drop zone
    public bool GrabItemsComplete(){
        return false;
    }
    // open box moved into completed drop zone
    public bool CompleteBoxComplete(){
        return false;
    }
    // end game pressed
    public bool EndComplete(){
        return false;
    }

    void GetNextItems(){
        
        // get all items and boxes left
        List<GameObject> allRemainingItems = itemManager.GetAllItems();
        List<GameObject> allRemainingBoxes = boxManager.GetAllUnopenBoxes();
        currentItems = new List<GameObject>();

        int numItemsRemaining = allRemainingItems.Count;
        int numBoxesRemaining = allRemainingBoxes.Count;

        int numNextItems = UnityEngine.Random.Range(1, Math.Min(numItemsRemaining - numBoxesRemaining, MAXITEMS + 1)); // TODO check math

        for(int i = 0; i< numNextItems; i++){
            int randomIndex = UnityEngine.Random.Range(0, allRemainingItems.Count);
            currentItems.Add(allRemainingItems[randomIndex]);
            allRemainingItems.RemoveAt(randomIndex);
        }       
    }
}
