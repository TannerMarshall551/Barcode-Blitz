using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextBackgroundBubble : MonoBehaviour
{
    public RectTransform background; // Drag the RectTransform of the background image here
    private TextMeshProUGUI tmpText;
    public Vector2 padding;
    public Vector2 sizeOffset;


    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();

        if(tmpText == null){
            Debug.LogError("No TMP Loaded");
        }
        if(background == null){
            Debug.LogError("No Background Image Loaded");
        }
        else{
            background.anchoredPosition -= new Vector2(padding.x, padding.y);
        }
    }

    public void UpdateBubble()
    {
        tmpText.ForceMeshUpdate(); // Force update if using auto-sizing to ensure accurate bounds

        // Update the size of the background to match the text
        background.sizeDelta = new Vector2(tmpText.renderedWidth + (padding.x * 2) + sizeOffset.x, tmpText.preferredHeight + (padding.y * 2) + sizeOffset.y);
    }
}
