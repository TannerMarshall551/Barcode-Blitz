using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class OrderPickerGameManager : MonoBehaviour
{
    public List<Bin> bins;
    public List<Package> packages = new();
    private Dictionary<string, string> targetPackagesUUIDToDisplayLabel = new();
    public ScannerItemManager scannerItemManager;
    public List<ScannerUIItem> scannerItems = new List<ScannerUIItem>();
    public int numPackagesToPick = 3;
    private List<string> ScannedUUIDs = new();

    void Start()
    {
        Bin[] binArray = FindObjectsOfType<Bin>();
        bins.AddRange(binArray);
        AssignLabels();
        SelectRandomLabels(numPackagesToPick);
        PopulateScanner();
        scannerItemManager.SetNewItems(scannerItems);
    }

    void AssignLabels()
    {
        foreach (Bin bin in bins)
        {
            bin.binsLabels = GetBinLabelStrings(bin);
            AssignPackageLabels(bin);
        }
    }

    private void AssignPackageLabels(Bin bin)
    {
        string pattern = @"\([^)]*\)";

        foreach (Transform child in bin.transform)
        {
            if (child.name.StartsWith("BoxIndicator"))
            {
                int indicatorNumber;
                string indicatorSuffix = child.name.Replace("BoxIndicator", "");
                if (int.TryParse(indicatorSuffix, out indicatorNumber))
                {
                    AssignLabelToPackage(child, bin, indicatorNumber, pattern);
                }
            }
        }
    }

    private void AssignLabelToPackage(Transform child, Bin bin, int indicatorNumber, string pattern)
    {
        Package package = child.GetComponentInChildren<Package>();
        if (package != null)
        {
            package.packageUUID = package.GetComponentInChildren<UUIDGenerator>().GetUUID();
            for (int j = 0; j < bin.binsLabels.Count; j++)
            {
                string modifiedLabel = Regex.Replace(bin.binsLabels[j], pattern, indicatorNumber.ToString());
                package.packageLabels.Add(modifiedLabel);
            }

            package.SetLabel(package.packageLabels[Random.Range(0, 2)]);

            bin.packages.Add(package);
            packages.Add(package);
        }
        else
        {
            Debug.LogError("Package component not found on " + child.name);
        }
    }

    private List<string> GetBinLabelStrings(Bin bin)
    {
        List<string> binLabelStrings = new List<string>();
        List<Transform> foundChildren = FindAllChildrenWithName(bin.transform, "BinLabel");
        foreach (Transform binLabelTransform in foundChildren)
        {
            string binLabel = binLabelTransform.Find("BinUUID").GetComponent<TextMeshPro>().text;
            binLabelStrings.Add(binLabel);
        }
        return binLabelStrings;
    }

    private void SelectRandomLabels(int numberOfSelections)
    {
        if (packages.Count < numberOfSelections)
        {
            Debug.LogError("Not enough packages to perform the selection.");
            return;
        }

        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < numberOfSelections)
        {
            int index = Random.Range(0, packages.Count);
            selectedIndices.Add(index);
        }

        Debug.Log($"Collect {numberOfSelections} Packages:");
        foreach (int index in selectedIndices)
        {
            Package package = packages[index];
            if (package.packageLabels.Count >= 2)
            {
                // Randomly select one of the two labels
                string selectedLabel = package.packageLabels[Random.Range(0, 2)];
                targetPackagesUUIDToDisplayLabel[package.packageUUID] = selectedLabel;
                package.SetLabel(selectedLabel);
                Debug.Log($"Label: {selectedLabel}, UUID: {package.packageUUID}");
            }
            else
            {
                Debug.LogError("Package does not have exactly two labels.");
            }
        }
    }

    List<Transform> FindAllChildrenWithName(Transform parent, string name)
    {
        List<Transform> resultList = new List<Transform>();
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                resultList.Add(child);
            }
            resultList.AddRange(FindAllChildrenWithName(child, name)); // Recursive search
        }
        return resultList;
    }

    public void PopulateScanner()
    {
        scannerItems.Clear();

        foreach (KeyValuePair<string, string> packageInfo in targetPackagesUUIDToDisplayLabel)
        {
            ScannerUIItem newItem = new ScannerUIItem();
            newItem.id = packageInfo.Key;

            if (ScannedUUIDs.Contains(packageInfo.Key))
            {
                newItem.pageColor = ScannerColorState.Complete;
            }
            else
            {
                newItem.pageColor = ScannerColorState.Default;
            }

            newItem.rows = new List<Row>();
            Row labelRow = new Row
            {
                type = RowType.Text,
                textRow = new TextRow
                {
                    headerText = "Package:",
                    bodyText = packageInfo.Value
                }
            };

            newItem.rows.Add(labelRow);
            Row uuidRow = new Row
            {
                type = RowType.Text,
                textRow = new TextRow
                {
                    headerText = "UUID:",
                    bodyText = packageInfo.Key
                }
            };

            newItem.rows.Add(uuidRow);
            scannerItems.Add(newItem);
        }

        scannerItemManager.SetNewItems(scannerItems);
    }

    public List<string> GetTargetUUIDs()
    {
        return targetPackagesUUIDToDisplayLabel.Keys.ToList();
    }

    public int AcknowledgeSuccessfulDelivery(string packageUUID)
    {
        targetPackagesUUIDToDisplayLabel.Remove(packageUUID);
        PopulateScanner();
        return numPackagesToPick - targetPackagesUUIDToDisplayLabel.Count();
    }

    public void ScanPackage(string packageUUID)
    {
        AddToScannedUUIDs(packageUUID);
        PopulateScanner();
        GoToLastScannedPackagePage(packageUUID);
    }

    private void GoToLastScannedPackagePage(string lastScannedPackageUUID)
    {
        int startIndex = scannerItemManager.GetIndex(); // Store the starting index to detect a full cycle
        string currentID = scannerItemManager.GetItemID();

        if (!GetTargetUUIDs().Contains(lastScannedPackageUUID))
        {
            return;
        }

        // If the first item is the target, no need to loop
        if (currentID == lastScannedPackageUUID)
        {
            return;
        }

        // Continue to the next item until we find the target or cycle through all items
        do
        {
            scannerItemManager.nextItem(); // Move to the next item
            currentID = scannerItemManager.GetItemID(); // Update the current ID

            if (currentID == lastScannedPackageUUID)
            {
                return;
            }

            // Check if we've returned to the start
        } while (scannerItemManager.GetIndex() != startIndex);

        Debug.LogWarning("Target package UUID not found in the current list after a full cycle.");
    }

    private void AddToScannedUUIDs(string UUID)
    {
        ScannedUUIDs.Add(UUID);
    }
}
