using UnityEngine;
using TMPro;

/*
 * Generates a unique UUID on each new Package prefab added to the scene
 * UUID is presented on label
 */

public class UUIDGenerator : MonoBehaviour
{
    private string uuid;

    private void Start()
    {
        // Generate UUID
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
            Debug.LogError("TextMeshPro component not found.");
        }
    }

    public string GetUUID()
    {
        return uuid;
    }
}
