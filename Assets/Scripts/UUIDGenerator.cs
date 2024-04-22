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
        // Generate UUID
        if (manualUUID.Equals(""))
        {
            // Debug.Log("HERE");
            uuid = System.Guid.NewGuid().ToString();

            // TextMeshPro component must be attached to child object of GameObject – this is the component that displays text
            // Find TextMeshPro component in child object -> set text to generated UUID
            TextMeshPro tmp = GetComponentInChildren<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = uuid;
            }
            else
            {
                // Debug.LogError("TextMeshPro component not found.");
            }
        }
        else
        {
            // Debug.Log("HERE INSTEAD: " + manualUUID);
            uuid = manualUUID;

            // TextMeshPro component must be attached to child object of GameObject – this is the component that displays text
            // Find TextMeshPro component in child object -> set text to generated UUID
            TextMeshPro tmp = GetComponentInChildren<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = uuid;
            }
            else
            {
                // Debug.LogError("TextMeshPro component not found.");
            }
        }
       
    }

    public string GetUUID()
    {
        return uuid;
    }
}
