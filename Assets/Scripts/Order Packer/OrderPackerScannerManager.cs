using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderPackerScannerManager : MonoBehaviour
{
    private List<string> allItems;
    private List<string> itemsRemaining;

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

        UpdateScanner(true);
    }

    // Update items
    public void UpdateItems(List<string> itemsRemaining){
        this.itemsRemaining = new List<string>(itemsRemaining);

        UpdateScanner(false);
    }

    // Updates the scanner
    public void UpdateScanner(bool newItems){
        
        itemPages = new List<ScannerUIItem>();

        List<string> uniqueItems = allItems.Distinct().ToList();
        
        foreach(string item in uniqueItems)
        {
            AddItemPage(item, allItems.Count(s => s == item), itemsRemaining.Count(d => d == item));
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
    public void AddItemPage(string item, int total, int remaining)
    {

        // item page
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = item;

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
        newRow.textRow.bodyText = (total - remaining).ToString();

        newItem.rows.Add(newRow);

        // row 4
        newRow = new Row();

        newRow.type = RowType.Selector;
        newRow.selectorRow = new SelectorRow();
        newRow.selectorRow.headerText = "Damaged?:";
        newRow.selectorRow.yesPressed = false;
        newRow.selectorRow.noPressed = false;

        newItem.rows.Add(newRow);

        // add to list
        itemPages.Add(newItem);
    }

    // method that is called when the start or complete package button is pressed
    public void StartCompletePressed(){
        gameManager.StartCompletePressed();
    }

}
