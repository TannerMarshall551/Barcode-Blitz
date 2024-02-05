using UnityEngine;
using TMPro;

public class UUIDGenerator : MonoBehaviour
{
    private void Start()
    {
        // Generate UUID
        string uuid = System.Guid.NewGuid().ToString();

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
}
