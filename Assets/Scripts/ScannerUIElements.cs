using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define an enum for row types
public enum RowType
{
    Text,
    Selector,
    Button
}

// Row that has two lines of text
[System.Serializable]
public class TextRow
{
    public string headerText;
    public string bodyText;
}

// Row that has one line of text and one yes no check box
[System.Serializable]
public class SelectorRow
{
    public string headerText;
    public bool yesPressed;
    public bool noPressed;
}

// Row that has one line of text in a button
[System.Serializable]
public class ButtonRow
{
    public string bodyText;
    public bool isPressed;
}

// Row for each part of the item page
[System.Serializable]
public class Row
{
    public RowType type;
    public TextRow textRow;
    public SelectorRow selectorRow;
    public ButtonRow buttonRow;
}

// ScannerUIItem contains the info for all the rows for each item
[System.Serializable]
public class ScannerUIItem
{
    public string id;
    public List<Row> rows = new List<Row>();

    // Copies newItem into current item
    public void CopyFrom(ScannerUIItem newItem){
        id = newItem.id;
        rows.Clear();

        // Copy each row from the newItem object
        foreach (var row in newItem.rows)
        {
            Row newRow = new Row();
            newRow.type = row.type;

            // Depending on the row type, copy the corresponding row data
            switch (row.type)
            {
                case RowType.Text:
                    newRow.textRow = new TextRow();
                    newRow.textRow.headerText = row.textRow.headerText;
                    newRow.textRow.bodyText = row.textRow.bodyText;
                    break;
                case RowType.Selector:
                    newRow.selectorRow = new SelectorRow();
                    newRow.selectorRow.headerText = row.selectorRow.headerText;
                    newRow.selectorRow.yesPressed = row.selectorRow.yesPressed;
                    newRow.selectorRow.noPressed = row.selectorRow.noPressed;
                    break;
                case RowType.Button:
                    newRow.buttonRow = new ButtonRow();
                    newRow.buttonRow.bodyText = row.buttonRow.bodyText;
                    newRow.buttonRow.isPressed = row.buttonRow.isPressed;
                    break;
            }

            // Add the new row to the list
            rows.Add(newRow);
        }
    }
}

