using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum GameState
{
    Tutorial, // Tutorial TODO
    StartPackage, // Click the scanner to start package
    MakeBox, // Make the box
    GrabItems, // Pick up items
    ScanItems, // Scan items held
    MarkTrashItems, // Mark items as trash if item is damaged
    DropItems, // Place items
    CompletePackage, // Click the scanner to complete package
    CompleteBox, // Add label to package
    SendPackage, // Move package to conveyer
    End // Prompt to retry TODO
}


/* 
========TODO=========
- Deduct points (add time) whenever a mistake was made
- Tutorial
- End

========BUGS=========
- Can pick up from item DZ when the item DZ is disabled
- Separate item pages are sometimes made for damaged items
    - However damaged and undamaged items can be used interchangeably for these pages
*/

public class OrderPackerGameManager : MonoBehaviour
{
    private GameState currentState; // current state the game is in

    // other managers
    private OrderPackerItemManager itemManager;
    private OrderPackerBoxManager boxManager;
    private OrderPackerDropZoneManager dropZoneManager;
    private OrderPackerScannerManager scannerManager;
    private OrderPackerUIManager uiManager;

    private List<string> currentItems; // current items (tags) for package

    private const int MAXITEMS = 5; // max number of items per package
    private const int MAXBOXES = 5; // max number of boxes

    private bool stateComplete = false; // if the current state is complete
    private bool damagedItem = false; // if the current item being held is damaged
    private bool itemScanned = false;
    private string currentItemTag = ""; // the tag for the current item

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.Tutorial;

        // Get the item manager, box manager, and scanner manager in scene
        GameObject boxManagerObj = GameObject.Find("BoxManager");
        GameObject itemManagerObj = GameObject.Find("ItemManager");
        GameObject dropZoneManagerObj = GameObject.Find("DropZoneManager");
        GameObject scannerManagerObj = GameObject.Find("ScannerManager");
        GameObject uiManagerObj = GameObject.Find("UIManager");

        if(boxManagerObj == null){
            Debug.LogError("No BoxManager Object");
        }
        if(itemManagerObj == null){
            Debug.LogError("No ItemManager Object");
        }
        if(dropZoneManagerObj == null){
            Debug.LogError("No DropZoneManager Object");
        }
        if(scannerManagerObj == null){
            Debug.LogError("No ScannerManager Object");
        }
        if(uiManagerObj == null){
            Debug.LogError("No UIManager Object");
        }
        
        
        boxManager = boxManagerObj.GetComponent<OrderPackerBoxManager>();
        itemManager = itemManagerObj.GetComponent<OrderPackerItemManager>();
        dropZoneManager = dropZoneManagerObj.GetComponent<OrderPackerDropZoneManager>();
        scannerManager = scannerManagerObj.GetComponent<OrderPackerScannerManager>();
        uiManager = uiManagerObj.GetComponent<OrderPackerUIManager>();


        if(boxManager == null){
            Debug.LogError("No BoxManager Loaded");
        }
        if(itemManager == null){
            Debug.LogError("No ItemManager Loaded");
        }
        if(dropZoneManager == null){
            Debug.LogError("No DropZoneManager Loaded");
        }
        if(scannerManager == null){
            Debug.LogError("No ScannerManager Loaded");
        }
        if(uiManager == null){
            Debug.LogError("No UIManager Loaded");
        }

        // spawn items and boxes
        itemManager.SpawnObjectsOnShelf();
        boxManager.SpawnBoxes(MAXBOXES);    

        LockAllDropZones();
    }

    // Update is called once per frame
    void Update()
    {
        // find the current state of the game
        switch (currentState)
        {
            case GameState.Tutorial:
                if(TutorialComplete())
                {
                    
                    Debug.Log("Tutorial Completed");
                    currentState = GameState.StartPackage;

                    scannerManager.StartPage();
                    dropZoneManager.UpdateDZVisability(false); 

                    uiManager.StartTimer();

                }
                break;
            case GameState.StartPackage:
                if(StartPackageComplete())
                {
                    Debug.Log("StartPackage Completed");
                    currentState = GameState.MakeBox;

                    boxManager.SetLockedBox(BoxState.Unmade, true, false);
                    boxManager.SetLockedBox(BoxState.Open, false, true);

                    GetNextItems();

                    scannerManager.SetItems(currentItems);
                    dropZoneManager.UpdateDZVisability(false);  
                }
                break;
            case GameState.MakeBox:
                if(MakeBoxComplete())
                {
                    Debug.Log("MakeBox Completed");
                    currentState = GameState.GrabItems;

                    boxManager.SetLockedBox(BoxState.Unmade, false, false);
                    boxManager.SetLockedBox(BoxState.Open, false, false);
                    itemManager.LockAllItems(true, true, false);
                    dropZoneManager.UpdateDZVisability(false); 

                    // AssignItemDZ();
                }
                break;
            case GameState.GrabItems:
                if(GrabItemsComplete())
                {
                    Debug.Log("GrabItems Completed");
                    
                    if(damagedItem){
                        currentState = GameState.MarkTrashItems;
                    }
                    else{
                        currentState = GameState.ScanItems;
                    }
                    
                    dropZoneManager.UpdateDZVisability(true); 
                }
                break;
            case GameState.MarkTrashItems:
                if(MarkTrashItemsComplete())
                {
                    Debug.Log("MarkTrashItems Completed");
                    currentState = GameState.DropItems;

                    itemManager.LockAllItems(false, false, true);
                    dropZoneManager.UpdateDZVisability(true);
                }
                break;
            case GameState.ScanItems:
                if(ScanItemsComplete())
                {
                    Debug.Log("ScanItems Completed");
                    currentState = GameState.DropItems;

                    boxManager.SetLockedBox(BoxState.Open, false, true);
                    itemManager.LockAllItems(false, false, false);
                    dropZoneManager.UpdateDZVisability(true); 
                }
                break;
            case GameState.DropItems:
                if(DropItemsComplete())
                {
                    Debug.Log("DropItems Completed");
                    if(currentItems.Count() == 0){
                        currentState = GameState.CompletePackage;

                        itemManager.LockAllItems(false, false, false);

                        scannerManager.UpdateItems(currentItems);
                        scannerManager.CompletePage();
                        dropZoneManager.UpdateDZVisability(false); 
                    }
                    else{
                        currentState = GameState.GrabItems;

                        boxManager.SetLockedBox(BoxState.Open, false, false);
                        itemManager.LockAllItems(true, true, false);

                        scannerManager.UpdateItems(currentItems);
                        dropZoneManager.UpdateDZVisability(false); 
                    }

                    currentItemTag = "";
                }
                break;
            case GameState.CompletePackage:
                if(CompletePackageComplete())
                {
                    Debug.Log("CompletePackage Completed");
                    currentState = GameState.CompleteBox;

                    boxManager.SetLockedPrinter(true, false);
                    boxManager.SetLockedBox(BoxState.Open, false, true);
                    dropZoneManager.UpdateDZVisability(false); 
                }
                break;
            case GameState.CompleteBox:
                if(CompleteBoxComplete())
                {
                    Debug.Log("CompleteBox Completed");
                    currentState = GameState.SendPackage;

                    boxManager.SetLockedBox(BoxState.Open, true, false);
                    boxManager.SetLockedBox(BoxState.Completed, false, true);
                    boxManager.SetLockedPrinter(false, false);
                    dropZoneManager.UpdateDZVisability(false); 
                }
                break;
            case GameState.SendPackage:
                if(SendPackageComplete())
                {
                    Debug.Log("SendPackage Completed");

                    LockAllDropZones();
                    boxManager.SetLockedBox(BoxState.Completed, true, false);
                    uiManager.SetCountText((MAXBOXES - boxManager.GetAllUnopenBoxes().Count()) + "/" + MAXBOXES);

                    if(boxManager.GetAllUnopenBoxes().Count > 0){ // check if there are more unopened boxes
                        Debug.Log("Back to StartPackage, next package");
                        currentState = GameState.StartPackage;
                        
                        scannerManager.StartPage();
                        dropZoneManager.UpdateDZVisability(false); 
                    }
                    else{
                        Debug.Log("All packages completed, to End");
                        currentState = GameState.End;
                        dropZoneManager.UpdateDZVisability(false); 

                        uiManager.StopTimer();
                    }
                }
                break;
            case GameState.End:
                if(EndComplete()){
                    // Prompt to start again or end TODO
                }
                break;
            default:
                Debug.LogWarning("Unknown GameState");
                break;
        } 
    }

    // checks if the state is complete and resets it
    public bool StateComplete(){
        if(stateComplete){
            stateComplete = false;
            return true;
        }
        return false;
    }

    // checks if space is pressed (temporary)
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
        return StateComplete();
    }

    // moved unopen box into next drop zone (opening the box)
    public bool MakeBoxComplete(){
        return StateComplete();
    }
    // all items in next drop zone
    public bool GrabItemsComplete(){
        return StateComplete();
    }
    // all items in next drop zone
    public bool ScanItemsComplete(){
        if(itemScanned){ // TODO scan item

            itemScanned = false;

            if(currentItemTag == scannerManager.GetCurrentItemPageID() && currentItems.Contains(currentItemTag)){
                return true; 
            }
            else{
                Debug.Log("Item doesn't match item page or item quantity already fulfilled!");
            }
        }
        return false;
    }
    //
    public bool MarkTrashItemsComplete(){
        if(ScanItemsComplete()){ // tried to scan trash item
            Debug.Log("Item is damaged! Mark as damaged and throw in trash.");
        }
        return StateComplete();
    }
    // all items in next drop zone
    public bool DropItemsComplete(){
        return StateComplete();
    }
    // open box moved into completed drop zone
    public bool CompletePackageComplete(){
        return StateComplete();
    }
    // open box moved into completed drop zone
    public bool CompleteBoxComplete(){
        return StateComplete(); 
    }
    // open box moved into completed drop zone
    public bool SendPackageComplete(){
         return StateComplete();
    }
    // end game pressed
    public bool EndComplete(){
        return false; // TODO
    }

    // sets currentItems to the next set of items
    void GetNextItems(){
        
        // get all items and boxes left
        List<GameObject> allRemainingItems = itemManager.GetAllItems();
        List<GameObject> allRemainingBoxes = boxManager.GetAllUnopenBoxes();
        
        // resets current items
        currentItems = new List<string>();

        int numItemsRemaining = allRemainingItems.Count;
        int numBoxesRemaining = allRemainingBoxes.Count;
        int numNextItems;
        
        if(numBoxesRemaining > 1){ // not last box
            numNextItems = UnityEngine.Random.Range(Math.Max((- (numBoxesRemaining - 1) * MAXITEMS + numItemsRemaining), 1), Math.Min(numItemsRemaining - numBoxesRemaining, MAXITEMS + 1));
        }
        else{ // last box
            numNextItems = numItemsRemaining;
        }

        // get random items
        for(int i = 0; i< numNextItems; i++){
            int randomIndex = UnityEngine.Random.Range(0, allRemainingItems.Count);
            currentItems.Add(allRemainingItems[randomIndex].tag.Replace(" Damaged", ""));
            allRemainingItems.RemoveAt(randomIndex);
        } 
    }

    // Called when the start and complete package buttons are pressed
    public void StartCompletePressed(){
        if(currentState == GameState.StartPackage || currentState == GameState.CompletePackage)
        stateComplete = true;
    }

    //
    public void MarkTrashPressed(string itemPageTag){


        if(currentState != GameState.MarkTrashItems){
            Debug.Log("Cannot mark as damaged, not correct game state!");
        }
        else if(itemPageTag == currentItemTag.Replace(" Damaged","") && currentItemTag.Contains(" Damaged")){ // item must be damaged at this point so just need to check if id's match
            stateComplete = true;
        }
        else{
            Debug.Log("Fix this");
        }
    }

    // Called when a box is placed
    public void BoxPlaced(ObjectGrabbableWithZones boxObj){
        stateComplete = true;
    }

    // Called when a label is placed
    public void LabelPlaced(){
        stateComplete = true;
    }

    // Called when an item is placed
    public void ItemPlaced(ObjectGrabbableWithZones itemObj){
        
        GameObject curDZ = itemObj.GetCurrentDropZone();

        if(curDZ == boxManager.GetZone(BoxState.Open) || curDZ == itemManager.garbadgeDZ) // item placed in open box
        { 
            if(currentItems.Remove(itemObj.tag.Replace(" Damaged",""))) // item in current items
            { 
                stateComplete = true;
                itemManager.RemoveItem(itemObj);
                // AssignItemDZ();

                if(damagedItem){
                    scannerManager.AddDamagedItem(itemObj.tag.Replace(" Damaged",""));
                }
            }
            else{ // item not in current items
                Debug.Log("Item not in list of current items");
            }
        }
        else{ // item placed back on shelf
            if(currentState != GameState.Tutorial){
                Debug.Log("Item placed back on shelf, back to GrabItems");
                currentState = GameState.GrabItems;
            }
        }

        damagedItem = false;
    }
    
    // called when an item is grabbed
    public void ItemGrabbed(ObjectGrabbableWithZones itemObj){

        stateComplete = true; // can only pick up items during GrabItems state

        currentItemTag = itemObj.gameObject.tag;

        if(currentItemTag.Contains(" Damaged")){
            damagedItem = true;
        }
        else{
            damagedItem = false;
        }
    }

    // TODO (remove?) add or remove the open box drop zone to items in current items
    public void AssignItemDZ(){

        // get all the items and distinct items in current items
        List<string> uniqueItems = currentItems.Distinct().ToList();
        List<GameObject> allRemainingItems = itemManager.GetAllItems();

        foreach(GameObject item in allRemainingItems)
        {
            ObjectGrabbableWithZones curItem = item.GetComponent<ObjectGrabbableWithZones>();

            if(curItem != null)
            {
                if(uniqueItems.Contains(item.tag)){ // item in current items
                    // curItem.AddDropZone(boxManager.GetZone(BoxState.Open));
                }
                else{ // item not in current items
                    // curItem.RemoveDropZone(boxManager.GetZone(BoxState.Open));
                }
            }
            else{
                Debug.Log("Not an item.");
            }
            
        }
    }

    // locks all drop zones
    public void LockAllDropZones(){
        boxManager.SetLockedBox(BoxState.Unmade, false, false);
        boxManager.SetLockedBox(BoxState.Open, false, false);
        boxManager.SetLockedBox(BoxState.Completed, false, false);
        boxManager.SetLockedPrinter(false, false);
        itemManager.LockAllItems(false, false, false);
    }

    //
    public void ScanItem(string uuid){
        itemScanned = true;
    }

    // 
}
