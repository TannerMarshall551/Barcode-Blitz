using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class OrderPickerGameManager : MonoBehaviour
{
    public List<Bin> bins;
    public List<Package> packages = new();
    private Dictionary<string, string> targetPackagesUUIDToDisplayLabel = new(); // <UUID, DisplayLabel>

    void Start()
    {
        Bin[] binArray = FindObjectsOfType<Bin>();
        bins.AddRange(binArray);
        Debug.Log("Found " + bins.Count + " bins in the scene.");
        AssignLabels();
        SelectRandomLabels(3);
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

        foreach (int index in selectedIndices)
        {
            Package package = packages[index];
            if (package.packageLabels.Count >= 2)
            {
                // Randomly select one of the two labels
                string selectedLabel = package.packageLabels[Random.Range(0, 2)];
                targetPackagesUUIDToDisplayLabel[package.packageUUID] = selectedLabel;
                package.SetLabel(selectedLabel);
                Debug.Log($"Selected Package UUID: {package.packageUUID}, Selected Label: {selectedLabel}");
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

    public List<string> GetTargetUUIDs()
    {
        return targetPackagesUUIDToDisplayLabel.Keys.ToList();
    }
}
