using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDZColorChanger : MonoBehaviour
{
    // Call this method to change the color of the floor and all its children
    public void ChangeColor(Color newColor)
    {
        // Change color of each child
        foreach (Transform child in transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.material.color = newColor;
            }
        }
    }
}
