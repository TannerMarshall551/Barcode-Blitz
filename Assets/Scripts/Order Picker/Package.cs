using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Package : MonoBehaviour
{
    public string packageUUID;
    public List<string> packageLabels = new List<string>();
    public string displayLabel;

    public void SetLabel(string label)
    {
        displayLabel = label;

        TextMeshPro[] tmps = this.GetComponentsInChildren<TextMeshPro>();
        if (tmps != null)
        {
            foreach (TextMeshPro tmp in tmps)
            {
                if (tmp.gameObject.name == "PackageLabel")
                {
                    tmp.text = label;
                }
            }
        }
        else
        {
            Debug.LogError("TextMeshPro components not found.");
        }
    }

}