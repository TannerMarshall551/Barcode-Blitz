using UnityEngine;
using TMPro;

/*
 * Generates a unique UUID on each new Package prefab added to the scene
 * UUID is presented on label
 */

public class UUIDGenerator : MonoBehaviour
{
    private string uuid;

    public string manualUUID;

    private void Awake()
    {
        // Check if a manual UUID is provided, use it; otherwise, generate a new UUID
        if (string.IsNullOrEmpty(manualUUID))
        {
            uuid = System.Guid.NewGuid().ToString();
            Debug.Log("Generated new UUID: " + uuid);
        }
        else
        {
            uuid = manualUUID;
            Debug.Log("Using manual UUID: " + uuid);
        }

        // TextMeshPro components must be attached to child objects of GameObject – these are the components that display text
        // Find TextMeshPro components in child objects -> set text to UUID
        TextMeshPro[] tmps = GetComponentsInChildren<TextMeshPro>();
        if (tmps.Length > 0)
        {
            foreach (TextMeshPro tmp in tmps)
            {
                if (tmp.gameObject.name == "BarcodeUUID")
                {
                    tmp.text = uuid;
                }
            }
        }
        else
        {
            Debug.LogError("TextMeshPro components not found.");
        }
    }

    public string GetUUID()
    {
        return uuid;
    }
}