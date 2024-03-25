using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ScannerItemPage displays all the rows for a given item
public class ScannerItemPage : MonoBehaviour
{

    /* SelectorRowEventHandler handles all events for the selector row button presses
        Used to set the actual values from the SelectorRow to the ScannerUIItem
    */
    public class SelectorRowEventHandler
    {
        public ScannerUISelectionRow selectorRowScript;
        public int rowIndex;

        public SelectorRowEventHandler(ScannerUISelectionRow selectorRowScript, int rowIndex){
            this.selectorRowScript = selectorRowScript;
            this.rowIndex = rowIndex;
        }
    }

    /* ButtonRowEventHandler handles all events for the button row button presses
        Used to set the actual values from the ButtonRow to the ScannerUIItem
    */
    public class ButtonRowEventHandler
    {
        public ScannerUIButtonRow buttonRowScript;
        public int rowIndex;

        public ButtonRowEventHandler(ScannerUIButtonRow buttonRowScript, int rowIndex){
            this.buttonRowScript = buttonRowScript;
            this.rowIndex = rowIndex;
        }
    }

    // variables for the event when an item is changed
    public delegate void ItemChangedDelegate(ScannerUIItem newItem);
    public event ItemChangedDelegate OnItemChanged;

    // game objects
    public ScannerUIItem item;
    public GameObject textRowPrefab;
    public GameObject selectorRowPrefab;
    public GameObject buttonRowPrefab;

    public float verticalSpacing = 0f;

    // list of all SelectorRowEventHandler (all selector rows have events)
    public List<SelectorRowEventHandler> selectorRowEventHandlers = new List<SelectorRowEventHandler>();
    public List<ButtonRowEventHandler> buttonRowEventHandlers = new List<ButtonRowEventHandler>();

    // Start is called before the first frame update
    void Start()
    {
        // Only load items if need be
        if(item != null){
            ReloadItem(item);
        } 
    }

    // On disable remove all children and any lingering events
    void OnDisable(){
        ClearChildren();
    }

    // Clears old item and loads the new items info
    public void ReloadItem(ScannerUIItem newItem = null){

        // make a local copy
        ScannerUIItem newItemTemp = new ScannerUIItem();
        newItemTemp.CopyFrom(newItem);

        // make sure item exists
        if (newItemTemp != null){
            
            // remove all children (rows)
            ClearChildren();
            item.CopyFrom(newItemTemp);

            // loads each row from the newItme
            for(int rowIndex=0; rowIndex < item.rows.Count; rowIndex++){
                
                // Setting local temp variables
                GameObject rowObject = null;
                Row row = item.rows[rowIndex];
                
                // match row type
                switch (row.type)
                {
                    case RowType.Text:
                        
                        // instantiate row object from the text row prefab and assign its values
                        rowObject = Instantiate(textRowPrefab, transform);
                        SetTextRowValues(rowObject, row.textRow);

                        break;
                    case RowType.Selector:

                        // instantiate row object from the selector row prefab and assign its values
                        rowObject = Instantiate(selectorRowPrefab, transform);
                        SetSelectorRowValues(rowObject, row.selectorRow);

                        // get the selector row script needed for event handling
                        ScannerUISelectionRow selectorRowScript = rowObject.GetComponent<ScannerUISelectionRow>();
                        
                        if (selectorRowScript != null){

                            // add the new selector row to the list of event handlers
                            SelectorRowEventHandler newHandler = new SelectorRowEventHandler(selectorRowScript, rowIndex);
                            selectorRowEventHandlers.Add(newHandler);
                        }

                        break;
                    case RowType.Button:

                        // instantiate row object from the button row prefab and assign its values
                        rowObject = Instantiate(buttonRowPrefab, transform);
                        SetButtonRowValues(rowObject, row.buttonRow);

                        // get the button row scripte needed for event handling
                        ScannerUIButtonRow buttonRowScript = rowObject.GetComponent<ScannerUIButtonRow>();

                        if(buttonRowScript != null){

                            // add the new selector row to the list of event handlers
                            ButtonRowEventHandler newHandler = new ButtonRowEventHandler(buttonRowScript, rowIndex);
                            buttonRowEventHandlers.Add(newHandler);
                        }
                        break;
                    default:
                        Debug.LogWarning("Unknown row type");
                        break;
                }
            }

            // loop through all selector rows add them to the event handler
            foreach (SelectorRowEventHandler handler in selectorRowEventHandlers){
                handler.selectorRowScript.OnYesPressedChanged += (newValue) => UpdateSelectorRowYesValue(handler.rowIndex, newValue);
                handler.selectorRowScript.OnNoPressedChanged += (newValue) => UpdateSelectorRowNoValue(handler.rowIndex, newValue);
            }

            // loop through all selector rows add them to the event handler
            foreach (ButtonRowEventHandler handler in buttonRowEventHandlers){
                handler.buttonRowScript.OnButtonChanged += (newValue) => UpdateButtonRowValue(handler.rowIndex, newValue);
            }
        }
        else{
            Debug.LogWarning("No item selected");
        }
    }

    // updates the yesPressed value for selector row
    void UpdateSelectorRowYesValue(int rowIndex, bool newValue){
        item.rows[rowIndex].selectorRow.yesPressed = newValue;
        CallOnItemChanged(); // announce changes
    }

    // updates the noPressed value for selector row
    void UpdateSelectorRowNoValue(int rowIndex, bool newValue){
        item.rows[rowIndex].selectorRow.noPressed = newValue;
        CallOnItemChanged(); // announce changes
    }

    // updates the noPressed value for selector row
    void UpdateButtonRowValue(int rowIndex, bool newValue){
        item.rows[rowIndex].buttonRow.isPressed = newValue;
        CallOnItemChanged(); // announce changes
    }

    // announce that an item has been edited
    void CallOnItemChanged(){
        if (OnItemChanged != null){
            OnItemChanged?.Invoke(item);
        }
    }

    // Removes all children for the current object (all the old objects rows)
    void ClearChildren(){
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (SelectorRowEventHandler handler in selectorRowEventHandlers){
            handler.selectorRowScript.OnYesPressedChanged -= (newValue) => UpdateSelectorRowYesValue(handler.rowIndex, newValue);
            handler.selectorRowScript.OnNoPressedChanged -= (newValue) => UpdateSelectorRowNoValue(handler.rowIndex, newValue);
        }

        // loop through all selector rows add them to the event handler
        foreach (ButtonRowEventHandler handler in buttonRowEventHandlers){
            handler.buttonRowScript.OnButtonChanged -= (newValue) => UpdateButtonRowValue(handler.rowIndex, newValue);
        }
    }

    // Sets the text of a given textRow based on the input row
    void SetTextRowValues(GameObject rowPrefab, TextRow textRow)
    {
        ScannerUITextRow textRowPrefabScript = rowPrefab.GetComponent<ScannerUITextRow>();
        if (textRowPrefabScript != null)
        {
            
            textRowPrefabScript.SetTextValues(textRow.headerText, textRow.bodyText);
        }
        else
        {
            Debug.LogError("ScannerUITextRow script not found on the prefab.");
        }
    }

    // Sets the values of a given selectorRow on the input row
    void SetSelectorRowValues(GameObject rowPrefab, SelectorRow selectorRow)
    {
        ScannerUISelectionRow selctionRowPrefabScript = rowPrefab.GetComponent<ScannerUISelectionRow>();
        if (selctionRowPrefabScript != null)
        {
            
            selctionRowPrefabScript.SetSelectionValues(selectorRow.headerText, selectorRow.yesPressed, selectorRow.noPressed);
        }
        else
        {
            Debug.LogError("ScannerUISelectionRow script not found on the prefab.");
        }
    }

    // Sets the values of a given buttonRow on the input row
    void SetButtonRowValues(GameObject rowPrefab, ButtonRow buttonRow)
    {
        ScannerUIButtonRow buttonRowPrefabScript = rowPrefab.GetComponent<ScannerUIButtonRow>();
        if (buttonRowPrefabScript != null)
        {
            
            buttonRowPrefabScript.SetButtonValue(buttonRow.bodyText, buttonRow.isPressed);
        }
        else
        {
            Debug.LogError("ScannerUIButtonRow script not found on the prefab.");
        }
    }
}
