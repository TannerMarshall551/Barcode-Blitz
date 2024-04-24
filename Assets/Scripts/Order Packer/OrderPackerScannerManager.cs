using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderPackerScannerManager : MonoBehaviour
{
    private List<string> allItems;
    private List<string> itemsRemaining;
    private List<string> damagedItems;

    public ScannerItemManager scannerItemManager; // get the scannerItemManager 
    public List<ScannerUIItem> itemPages; 

    // Game manager
    public OrderPackerGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if(gameManager == null){
            Debug.LogError("Game Manager not loaded!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Resets scanner to start page
    public void StartPage(){

        itemPages = new List<ScannerUIItem>();
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "start";
        newItem.pageColor = ScannerColorState.Default;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Button;
        newRow.buttonRow = new ButtonRow();
        newRow.buttonRow.bodyText = "Start Package";
        newRow.buttonRow.isPressed = false;

        newItem.rows.Add(newRow);
        itemPages.Add(newItem);

        scannerItemManager.SetNewItems(itemPages);
    }

    // Resets scanner to start page
    public void CompletePage(){

        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "complete";
        newItem.pageColor = ScannerColorState.Default;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Button;
        newRow.buttonRow = new ButtonRow();
        newRow.buttonRow.bodyText = "Complete Package";
        newRow.buttonRow.isPressed = false;

        newItem.rows.Add(newRow);
        itemPages.Add(newItem);

        int index = scannerItemManager.GetIndex();
        scannerItemManager.SetNewItems(itemPages, index);
    }

    // Resets scanner to start page
    public void EndPage(){

        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "end";
        newItem.pageColor = ScannerColorState.Default;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Button;
        newRow.buttonRow = new ButtonRow();
        newRow.buttonRow.bodyText = "Complete Package";
        newRow.buttonRow.isPressed = false;

        newItem.rows.Add(newRow);
        itemPages.Add(newItem);

        int index = scannerItemManager.GetIndex();
        scannerItemManager.SetNewItems(itemPages, index);
    }

    // Get new items
    public void SetItems(List<string> newItems){
        allItems = new List<string>(newItems);
        itemsRemaining = allItems;
        damagedItems = new List<string>();

        UpdateScanner(true);
    }

    // Update items
    public void UpdateItems(List<string> itemsRemaining){
        this.itemsRemaining = new List<string>(itemsRemaining);

        UpdateScanner(false);
    }

    // Adds a damaged item
    public void AddDamagedItem(string itemID){
        damagedItems.Add(itemID);
    }

    // Updates the scanner
    public void UpdateScanner(bool newItems){
        
        itemPages = new List<ScannerUIItem>();

        List<string> uniqueItems = allItems.Distinct().ToList();
        
        foreach(string item in uniqueItems)
        {
            AddItemPage(item, allItems.Count(s => s == item), itemsRemaining.Count(d => d == item), damagedItems.Count(f => f == item));
        }
        
        int index = scannerItemManager.GetIndex();
        
        if(newItems){
            scannerItemManager.SetNewItems(itemPages);
        }
        else{
            scannerItemManager.SetNewItems(itemPages, index);
        }
    }

    // Adds an item page to the scanner
    public void AddItemPage(string item, int total, int remaining, int damaged)
    {

        // item page
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = item;
        if(total == remaining){
            newItem.pageColor = ScannerColorState.Default;
        }
        else if(remaining > 0){
            newItem.pageColor = ScannerColorState.InProgress;
        }
        else{
            newItem.pageColor = ScannerColorState.Complete;
        }

        // row 1
        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Product:";
        newRow.textRow.bodyText = item;

        newItem.rows.Add(newRow);

        // row 2
        newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Ordered:";
        newRow.textRow.bodyText = total.ToString();

        newItem.rows.Add(newRow);

        // row 3
        newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Packed:";
        if(damaged > 0){
            newRow.textRow.bodyText = (total - remaining).ToString() + " - ( " + (damaged).ToString() +" damaged )";
        }
        else{
            newRow.textRow.bodyText = (total - remaining).ToString();
        }
        newItem.rows.Add(newRow);

        // row 4
        newRow = new Row();

        newRow.type = RowType.Button;
        newRow.buttonRow = new ButtonRow();
        newRow.buttonRow.bodyText = "Mark as Damaged";
        newRow.buttonRow.isPressed = false;

        newItem.rows.Add(newRow);

        // add to list
        itemPages.Add(newItem);
    }

    // method that is called when the start or complete package button is pressed
    public void StartCompletePressed(){
        gameManager.StartCompletePressed();
    }

    //
    public void ItemUpdated(ScannerUIItem newScannerUIItem){
        if(newScannerUIItem.id == "start" || newScannerUIItem.id == "complete"){
            StartCompletePressed();
        }
        else{
            gameManager.MarkTrashPressed(newScannerUIItem.id);
        }
    }

    public string GetCurrentItemPageID(){
        return scannerItemManager.GetItemID();
    }
}
