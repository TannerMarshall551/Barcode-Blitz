using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SampleScannerUIItemPages : MonoBehaviour
{

    public ScannerItemManager scannerItemManager; // get the scannerItemManager 
    public List<ScannerUIItem> exampleItems = new List<ScannerUIItem>(); // my example list of items

    // Start is called before the first frame update
    void Start()
    {
        // creating my example items
        AddOrderPackerSample();
        AddOrderPickerSample();
        AddReturnsProcessorSample();

        // setting the new items for the item manager
        scannerItemManager.SetNewItems(exampleItems);
    }

    // add a sample order packer page
    private void AddOrderPackerSample(){
        
        // sample page
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "OrderPacker1";

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Sample";
        newRow.textRow.bodyText = "OrderPacker";

        newItem.rows.Add(newRow);
        exampleItems.Add(newItem);

        // item page
        newItem = new ScannerUIItem();
        newItem.id = "OrderPacker2";

        // row 1
        newItem.rows = new List<Row>();
        newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Product:";
        newRow.textRow.bodyText = "Hair Brush";

        newItem.rows.Add(newRow);

        // row 2
        newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Ordered:";
        newRow.textRow.bodyText = "2";

        newItem.rows.Add(newRow);

        // row 3
        newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Packed:";
        newRow.textRow.bodyText = "1";

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
        exampleItems.Add(newItem);
    }

    // add a sample order picker
    private void AddOrderPickerSample(){
        
        // sample page
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "OrderPicker1";

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Sample";
        newRow.textRow.bodyText = "OrderPicker";

        newItem.rows.Add(newRow);

        exampleItems.Add(newItem);

        // item page
        newItem = new ScannerUIItem();
        newItem.id = "OrderPicker2";

        // row 1
        newItem.rows = new List<Row>();
        newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Product:";
        newRow.textRow.bodyText = "Hair Brush";

        newItem.rows.Add(newRow);

        // row 2
        newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "To Pick:";
        newRow.textRow.bodyText = "2";

        newItem.rows.Add(newRow);

        // row 3
        newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Location:";
        newRow.textRow.bodyText = "ABC";

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
        exampleItems.Add(newItem);
    }

    // add a sample returns processor
    private void AddReturnsProcessorSample(){
        
        // sample page
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.id = "ReturnsProcessor1";

        newItem.rows = new List<Row>();
        Row newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Sample";
        newRow.textRow.bodyText = "ReturnsProcessor";

        newItem.rows.Add(newRow);

        exampleItems.Add(newItem);

        // item page
        newItem = new ScannerUIItem();
        newItem.id = "ReturnsProcessor2";

        // row 1
        newItem.rows = new List<Row>();
        newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Product:";
        newRow.textRow.bodyText = "Hair Brush";

        newItem.rows.Add(newRow);

        // row 2
        newRow = new Row();

        newRow.type = RowType.Selector;
        newRow.selectorRow = new SelectorRow();
        newRow.selectorRow.headerText = "Damaged?:";
        newRow.selectorRow.yesPressed = false;
        newRow.selectorRow.noPressed = false;

        newItem.rows.Add(newRow);

        // row 3
        newRow = new Row();

        newRow.type = RowType.Text;
        newRow.textRow = new TextRow();
        newRow.textRow.headerText = "Location:";
        newRow.textRow.bodyText = "ABC";

        newItem.rows.Add(newRow);

        // add to list
        exampleItems.Add(newItem);
    }
}
