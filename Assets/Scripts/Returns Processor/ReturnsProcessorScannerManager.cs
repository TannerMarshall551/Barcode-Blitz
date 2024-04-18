using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnsProcessorScannerManager : MonoBehaviour
{
    private List<string> allItems;
    private List<string> itemsRemaining;
    private List<string> damagedItems;

    public RPScannerManager scannerItemManager; // get the scannerItemManager 
    public List<ScannerUIItem> itemPages;

    // Game manager
    public ReturnsProcessorGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("Game Manager not loaded!");
        }
        itemPages = new List<ScannerUIItem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Resets scanner to start page
    public void StartPage()
    {
        itemPages.Clear();
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "start";
        newItem.pageColor = ScannerColorState.Default;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Scan Package";
        newRow.textRow.bodyText = "Waiting . . .";

        newItem.rows.Add(newRow);
        itemPages.Add(newItem);

        //FIGURE OUT WHAT THIS DOES
        scannerItemManager.SetNewItems(itemPages);
    }

   
    public void MarkIfCorrectPage()
    {
        itemPages.Clear();
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "markCorrect";
        newItem.pageColor = ScannerColorState.Default;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Item";
        string tmp = gameManager.GetCarTypeForScanner();
        newRow.textRow.bodyText = tmp;
        newItem.rows.Add(newRow);

        Row newRow2 = new Row();
        newRow2.type = RowType.Text;
        newRow2.textRow = new TextRow();
        newRow2.textRow.headerText = "Is Item Correct?";
        newItem.rows.Add(newRow2);

        Row newButtonRow = new Row();
        newButtonRow.type = RowType.Selector;
        newButtonRow.selectorRow = new SelectorRow();
        newButtonRow.selectorRow.noPressed = false;
        newButtonRow.selectorRow.yesPressed = false;
        newItem.rows.Add(newButtonRow);

        itemPages.Add(newItem);


        scannerItemManager.SetNewItems(itemPages);
    }

    public void Discard()
    {
        itemPages.Clear();
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "discard";
        newItem.pageColor = ScannerColorState.Default;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Incorrect Item";
        newRow.textRow.bodyText = "\n\nPlace item and box in trash";
        newItem.rows.Add(newRow);

        itemPages.Add(newItem);
        scannerItemManager.SetNewItems(itemPages);
    }

    public void CorrectItemPage()
    {
        itemPages.Clear();
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "correctItem";
        newItem.pageColor = ScannerColorState.Default;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Scan Bin then Place Item Inside";
        newRow.textRow.bodyText = "\n\n\n\nBin: " + gameManager.GetBinLocationForScanner();
        newItem.rows.Add(newRow);

        itemPages.Add(newItem);
        scannerItemManager.SetNewItems(itemPages);
    }

    public void CorrectItemPageRed()
    {
        itemPages.Clear();
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "correctItem";
        newItem.pageColor = ScannerColorState.ScanFailed;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Scan Bin then Place Item Inside";
        newRow.textRow.bodyText = "\n\n\n\nBin: " + gameManager.GetBinLocationForScanner();
        newItem.rows.Add(newRow);

        itemPages.Add(newItem);
        scannerItemManager.SetNewItems(itemPages);
    }

    public void CorrectItemPageGreen()
    {
        itemPages.Clear();
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "correctItem";
        newItem.pageColor = ScannerColorState.Complete;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Scan Bin then Place Item Inside";
        newRow.textRow.bodyText = "\n\n\n\nBin: " + gameManager.GetBinLocationForScanner();
        newItem.rows.Add(newRow);

        itemPages.Add(newItem);
        scannerItemManager.SetNewItems(itemPages);
    }

    public void DiscardBoxPage()
    {
        itemPages.Clear();
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "discardBox";
        newItem.pageColor = ScannerColorState.Default;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Discard Empty Box";
        newItem.rows.Add(newRow);

        itemPages.Add(newItem);
        scannerItemManager.SetNewItems(itemPages);
    }

    public void YouWinPage()
    {
        itemPages.Clear();
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "youWin";
        newItem.pageColor = ScannerColorState.Default;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "YOU WIN!!!";
        newItem.rows.Add(newRow);

        itemPages.Add(newItem);
        scannerItemManager.SetNewItems(itemPages);
    }

    public void YouLosePage()
    {
        itemPages.Clear();
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "youLose";
        newItem.pageColor = ScannerColorState.Default;

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Sorry, You Ran Out of Time :(";
        newItem.rows.Add(newRow);

        itemPages.Add(newItem);
        scannerItemManager.SetNewItems(itemPages);
    }

    // Get new items
    public void SetItems(List<string> newItems)
    {
        allItems = new List<string>(newItems);
        itemsRemaining = allItems;
        damagedItems = new List<string>();

        UpdateScanner(true);
    }

    // Update items
    public void UpdateItems(List<string> itemsRemaining)
    {
        this.itemsRemaining = new List<string>(itemsRemaining);

        UpdateScanner(false);
    }

    // Adds a damaged item
    public void AddDamagedItem(string itemID)
    {
        damagedItems.Add(itemID);
    }

    // Updates the scanner
    public void UpdateScanner(bool newItems)
    {

        itemPages = new List<ScannerUIItem>();

       // List<string> uniqueItems = allItems.Distinct().ToList();

       // foreach (string item in uniqueItems)
        {
            //AddItemPage(item, allItems.Count(s => s == item), itemsRemaining.Count(d => d == item), damagedItems.Count(f => f == item));
        }

        int index = scannerItemManager.GetIndex();

        if (newItems)
        {
            scannerItemManager.SetNewItems(itemPages);
        }
        else
        {
            scannerItemManager.SetNewItems(itemPages, index);
        }
    }

    // Adds an item page to the scanner
    public void AddItemPage(string item, int total, int remaining, int damaged)
    {

        // item page
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = item;
        if (total == remaining)
        {
            newItem.pageColor = ScannerColorState.Default;
        }
        else if (remaining > 0)
        {
            newItem.pageColor = ScannerColorState.InProgress;
        }
        else
        {
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
        if (damaged > 0)
        {
            newRow.textRow.bodyText = (total - remaining).ToString() + " - ( " + (damaged).ToString() + " damaged )";
        }
        else
        {
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
    public void StartCompletePressed()
    {
       // gameManager.StartCompletePressed();
    }

    //
    public void ItemUpdated(ScannerUIItem newScannerUIItem)
    {
        if (newScannerUIItem.id == "markCorrect")
        {
            List<Row> rowList = newScannerUIItem.rows;
            foreach(Row row in rowList)
            {
                if(row.type == RowType.Selector)
                {
                    if(row.selectorRow.yesPressed)
                    {
                        gameManager.CheckIfCorrectButton(0);
                    }
                    else if(row.selectorRow.noPressed)
                    {
                        gameManager.CheckIfCorrectButton(1);
                    }
                }
            }
        }
        else
        {
           // gameManager.MarkTrashPressed(newScannerUIItem.id);
        }
    }

    public string GetCurrentItemPageID()
    {
        return scannerItemManager.GetItemID();
    }
}
