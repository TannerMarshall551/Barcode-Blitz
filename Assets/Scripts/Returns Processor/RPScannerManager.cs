using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPScannerManager : MonoBehaviour
{
    // list of each item for your tast
    public List<ScannerUIItem> scannerUIItems = new List<ScannerUIItem>();

    // game object
    //public TextMeshProUGUI itemNumber; // the current item (1 of 5)
   // public Button prevItemButton;
    //public Button nextItemButton;
    public ScannerItemPage itemPage; // current page for the current item

    private int currentItemIndex = 0;

    public ReturnsProcessorScannerManager scannerManager;

    // Start is called before the first frame update
    void Start()
    {
        if (scannerManager == null)
        {
            Debug.LogError("Scanner Manager not loaded!");
        }
        itemPage.OnItemChanged += (newItem) => UpdateCurrentItem(newItem); // add to item page to event

        LoadItem();
    }

    // On disable
    void OnDisable()
    {

        //itemPage.OnItemChanged -= (newItem) => UpdateCurrentItem(newItem); // remove from item page event
    }

    // gets the current index
    public int GetIndex()
    {
        return currentItemIndex;
    }

    // function to switch to next item in list
    public void nextItem()
    {

        // make sure not on last item
        if (currentItemIndex < scannerUIItems.Count - 1)
        {

            // set values for next item
            currentItemIndex++;
            itemPage.ReloadItem(scannerUIItems[currentItemIndex]);
           // itemNumber.text = (currentItemIndex + 1).ToString() + "/" + scannerUIItems.Count;
        }
        else
        {
            Debug.LogWarning("Can't go forward anymore, on last item");
        }

        UpdateButtonVisibility();
    }

    // function to switch to previous item in list
    public void prevItem()
    {

        // make sure not on first item
        if (currentItemIndex > 0)
        {

            // set values for previous item
            currentItemIndex--;
            itemPage.ReloadItem(scannerUIItems[currentItemIndex]);
           // itemNumber.text = (currentItemIndex + 1).ToString() + "/" + scannerUIItems.Count;
        }
        else
        {
            Debug.LogWarning("Can't go back any further, no more items");
        }

        UpdateButtonVisibility();
    }

    // function to only show valid buttons
    void UpdateButtonVisibility()
    {
        // if not on first item
        if (currentItemIndex > 0)
        {
          //  prevItemButton.gameObject.SetActive(true);
        }
        else
        {
          //  prevItemButton.gameObject.SetActive(false);
        }

        // if not on last item
        if (currentItemIndex < scannerUIItems.Count - 1)
        {
           // nextItemButton.gameObject.SetActive(true);
        }
        else
        {
           // nextItemButton.gameObject.SetActive(false);
        }
    }

    // Loads all items in scanner from start
    public void LoadItem(int index = 0)
    {

        // make sure there are items
        if (scannerUIItems.Count > 0)
        {

            // set values for first item
            currentItemIndex = index;
            itemPage.ReloadItem(scannerUIItems[currentItemIndex]);
           // itemNumber.text = (currentItemIndex + 1).ToString() + "/" + scannerUIItems.Count;
        }

        UpdateButtonVisibility();
    }

    // updates the current item to match new item
    public void UpdateCurrentItem(ScannerUIItem newScannerUIItem)
    {

        scannerManager.ItemUpdated(newScannerUIItem);
        ScannerUIItem newItem = new ScannerUIItem();
        newItem.CopyFrom(newScannerUIItem); // copy new item into local copy
        scannerUIItems[currentItemIndex] = newItem;
    }

    // takes a list of items and copies them into scannerUIItems
    public void SetNewItems(List<ScannerUIItem> newScannerUIItems, int index = 0)
    {

        scannerUIItems.Clear();

        foreach (ScannerUIItem curItem in newScannerUIItems)
        {
            ScannerUIItem newItem = new ScannerUIItem();
            newItem.CopyFrom(curItem);
            scannerUIItems.Add(newItem);
        }

        LoadItem(index);
    }

    //
    public string GetItemID()
    {
        return itemPage.GetItem().id;
    }
}
