using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum GameState
{
    Tutorial, // Tutorial TODO
    StartPackage, // Click the scanner to start package TODO
    MakeBox, // Make the box
    GrabItems, // Pick up items
    ScanItems, // Scan items held TODO
    DropItems, // Place items
    CompletePackage, // Click the scanner to complete package TODO
    CompleteBox, // Add label to package
    SendPackage, // Move package to conveyer
    End // Prompt to retry TODO
}

public class OrderPackerGameManager : MonoBehaviour
{
    private GameState currentState;

    private OrderPackerItemManager itemManager;
    private OrderPackerBoxManager boxManager;

    public ScannerItemManager scannerManager;

    private List<string> currentItems;

    private const int MAXITEMS = 5;

    private bool stateComplete = false;

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

        LockAllDropZones();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.Tutorial:
                if(TutorialComplete()){
                    Debug.Log("Tutorial Completed");
                    currentState = GameState.StartPackage;
                }
                break;
            case GameState.StartPackage:
                if(StartPackageComplete()){
                    Debug.Log("StartPackage Completed");
                    currentState = GameState.MakeBox;

                    boxManager.SetLockedBox(BoxState.Unmade, true, true);
                    boxManager.SetLockedBox(BoxState.Open, false, true);

                    GetNextItems();
                }
                break;
            case GameState.MakeBox:
                if(MakeBoxComplete()){
                    Debug.Log("MakeBox Completed");
                    currentState = GameState.GrabItems;

                    boxManager.SetLockedBox(BoxState.Unmade, false, false);
                    boxManager.SetLockedBox(BoxState.Open, false, false);
                    itemManager.LockAllItems(true, true);

                    AssignItemDZ();
                }
                break;
            case GameState.GrabItems:
                if(GrabItemsComplete()){
                    Debug.Log("GrabItems Completed");
                    currentState = GameState.ScanItems;
                }
                break;
            case GameState.ScanItems:
                if(ScanItemsComplete()){
                    Debug.Log("ScanItems Completed");
                    currentState = GameState.DropItems;

                    boxManager.SetLockedBox(BoxState.Open, false, true);
                    itemManager.LockAllItems(false, false);
                }
                break;
            case GameState.DropItems:
                if(DropItemsComplete()){
                    Debug.Log("DropItems Completed");
                    if(currentItems.Count() == 0){
                        currentState = GameState.CompletePackage;

                        itemManager.LockAllItems(false, false);
                    }
                    else{
                        currentState = GameState.GrabItems;

                        boxManager.SetLockedBox(BoxState.Open, false, false);
                        itemManager.LockAllItems(true, true);
                    }
                }
                break;
            case GameState.CompletePackage:
                if(CompletePackageComplete()){
                    Debug.Log("CompletePackage Completed");
                    currentState = GameState.CompleteBox;
                }
                break;
            case GameState.CompleteBox:
                if(CompleteBoxComplete()){
                    Debug.Log("CompleteBox Completed");
                    currentState = GameState.SendPackage;

                    boxManager.SetLockedBox(BoxState.Open, true, true);
                    boxManager.SetLockedBox(BoxState.Completed, true, true);
                }
                break;
            case GameState.SendPackage:
                if(SendPackageComplete()){
                    Debug.Log("SendPackage Completed");
                    LockAllDropZones();
                    if(boxManager.GetAllUnopenBoxes().Count > 0){
                        Debug.Log("Back to StartPackage, next package");
                        currentState = GameState.StartPackage;
                    }
                    else{
                        Debug.Log("All packages completed, to End");
                        currentState = GameState.End;
                    }
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

    public bool StateComplete(){
        if(stateComplete){
            stateComplete = false;
            return true;
        }
        return false;
    }

    public bool SpacePressed(){
        if(Input.GetKeyDown(KeyCode.Space)){
            return true;
        }
        return false;
    }

    // tutorial completed
    public bool TutorialComplete(){
        return SpacePressed(); // TODO
    }
    // start button pressed
    public bool StartPackageComplete(){
        return SpacePressed(); // TODO
    }

    // moved unopen box into next drop zone (opening the box)
    public bool MakeBoxComplete(){
        return StateComplete();
    }
    // all items in next drop zone
    public bool GrabItemsComplete(){
        return StateComplete(); //TODO
    }
    // all items in next drop zone
    public bool ScanItemsComplete(){
        return SpacePressed();
    }
    // all items in next drop zone
    public bool DropItemsComplete(){
        return StateComplete();
    }
    // open box moved into completed drop zone
    public bool CompletePackageComplete(){
        return SpacePressed(); // TODO
    }
    // open box moved into completed drop zone
    public bool CompleteBoxComplete(){
        return SpacePressed(); // TODO
    }
    // open box moved into completed drop zone
    public bool SendPackageComplete(){
        if(stateComplete){
            stateComplete = false;
            return true;
        }
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
        currentItems = new List<string>();

        int numItemsRemaining = allRemainingItems.Count;
        int numBoxesRemaining = allRemainingBoxes.Count;
        int numNextItems;
        
        if(numBoxesRemaining > 1){
            numNextItems = UnityEngine.Random.Range(1, Math.Min(numItemsRemaining - numBoxesRemaining, MAXITEMS + 1)); // TODO check math
        }
        else{
            numNextItems = numItemsRemaining;
        }

        for(int i = 0; i< numNextItems; i++){
            int randomIndex = UnityEngine.Random.Range(0, allRemainingItems.Count);
            currentItems.Add(allRemainingItems[randomIndex].tag);
            allRemainingItems.RemoveAt(randomIndex);
        }

        Debug.Log(string.Join(", ", currentItems));       
    }

    // Called when a box is placed
    public void BoxPlaced(ObjectGrabbableWithZones boxObj){

        GameObject curDZ = boxObj.GetCurrentDropZone();

        switch (currentState)
        {
            case GameState.MakeBox:
                if(curDZ == boxManager.GetZone(BoxState.Open)){
                    stateComplete = true;
                }
                break;
            case GameState.SendPackage:
                if(curDZ == boxManager.GetZone(BoxState.Completed)){
                    stateComplete = true;
                }
                break;
            default:
                break;
        }
    }

    public void ItemPlaced(ObjectGrabbableWithZones itemObj){
        
        GameObject curDZ = itemObj.GetCurrentDropZone();

        // TODO Unhighlight all drop zones avaliable when item is picked up

        if(curDZ == boxManager.GetZone(BoxState.Open)){ // item placed in open box
            if(currentItems.Remove(itemObj.tag)){
                stateComplete = true;
                itemManager.RemoveItem(itemObj);
                AssignItemDZ();
            }
            else{
                Debug.Log("Item not in list of current items");
            }
        }
        else{ // item placed back on shelf
            if(currentState != GameState.Tutorial){
                Debug.Log("Item placed back on shelf, back to GrabItems");
                currentState = GameState.GrabItems;
            }
        }


        if(currentItems != null){
            Debug.Log(string.Join(", ", currentItems));
        }

    }

    public void ItemGrabbed(ObjectGrabbableWithZones itemObj){

        stateComplete = true; // can only pick up items during GrabItems state

        // TODO Highlight all drop zones avaliable when item is picked up
    }

    public void AssignItemDZ(){
        List<string> uniqueItems = currentItems.Distinct().ToList();
        List<GameObject> allRemainingItems = itemManager.GetAllItems();

        foreach(GameObject item in allRemainingItems){
            ObjectGrabbableWithZones curItem = item.GetComponent<ObjectGrabbableWithZones>();
            if(curItem != null){
                if(uniqueItems.Contains(item.tag)){
                    curItem.AddDropZone(boxManager.GetZone(BoxState.Open));
                }
                else{
                    curItem.RemoveDropZone(boxManager.GetZone(BoxState.Open));
                }
            }
            else{
                Debug.Log("Not an item.");
            }
            
        }
    }

    public void LockAllDropZones(){
        boxManager.SetLockedBox(BoxState.Unmade, false, false);
        boxManager.SetLockedBox(BoxState.Open, false, false);
        boxManager.SetLockedBox(BoxState.Completed, false, false);
        itemManager.LockAllItems(false, false);
    }
}
