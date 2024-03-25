using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// logic for selectorRow prefab
public class ScannerUIButtonRow : MonoBehaviour
{

    // variables for the on button press events
    public delegate void BoolChangedDelegate(bool newValue);
    public event BoolChangedDelegate OnButtonChanged;

    // game objects
    public TMP_Text body;
    public Button button;

    // variables used to update game objects
    public string bodyText;
    public bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Making sure each component is assigned
        if (body == null){
            Debug.LogError("Body component is not assigned");
        }
        if (button == null){
            Debug.LogError("Button component is not assigned");
        }

        // add yes and no button event handlers
        button.onClick.AddListener(() => ToggleButtonState());

        // sets all the assigned values to the row game object
        SetButton();
    }

    // method to update the values for the row game object
    public void SetButtonValue(string bodyText, bool isPressed)
    {
        this.bodyText = bodyText;
        this.isPressed = isPressed;

        SetButton();
    }

    // sets all the values for the row game object
    private void SetButton(){

        body.text = bodyText;

        // if the yes button is pressed
        if (isPressed){
            PressButton();
        }
        else{
            ReleaseButton();
        }
    }

    // toggles the button states when one is clicked (only one can be pressed down at a time)
    void ToggleButtonState()
    {
        // Determine which button was clicked and toggle both buttons states
        if (isPressed)
        {
            ReleaseButton();
        }
        else
        {
            PressButton();
        }

        // announce that the button states have been changed
        CallOnPressed();

    }

    // logic for pressing a button down
    void PressButton()
    {
        // Press the button and update its visual appearance
        isPressed = true;
        button.interactable = false;
    }

    // logic for releasing a button
    void ReleaseButton()
    {
        // Release the button and update its visual appearance
        isPressed = false;
        button.interactable = true;
    }

    // invoke the on yes and no pressed changed events
    void CallOnPressed(){
        OnButtonChanged?.Invoke(isPressed);
    }
}
