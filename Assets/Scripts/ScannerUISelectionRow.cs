using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// logic for selectorRow prefab
public class ScannerUISelectionRow : MonoBehaviour
{

    // variables for the on button press events
    public delegate void BoolChangedDelegate(bool newValue);
    public event BoolChangedDelegate OnYesPressedChanged;
    public event BoolChangedDelegate OnNoPressedChanged;

    // game objects
    public TMP_Text header;
    public Button yesButton;
    public Button noButton;

    // variables used to update game objects
    public string headerText;
    public bool yesPressed = false;
    public bool noPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Making sure each component is assigned
        if (header == null){
            Debug.LogError("Header component is not assigned");
        }
        if (yesButton == null){
            Debug.LogError("Header component is not assigned");
        }
        if (noButton == null){
            Debug.LogError("Header component is not assigned");
        }

        // add yes and no button event handlers
        yesButton.onClick.AddListener(() => ToggleButtonState(yesButton));
        noButton.onClick.AddListener(() => ToggleButtonState(noButton));

        // sets all the assigned values to the row game object
        SetSelection();
    }

    // method to update the values for the row game object
    public void SetSelectionValues(string headerText, bool yesPressed, bool noPressed)
    {
        this.headerText = headerText;
        this.yesPressed = yesPressed;
        this.noPressed = noPressed;

        SetSelection();
    }

    // sets all the values for the row game object
    private void SetSelection(){

        header.text = headerText;

        // if the yes button is pressed
        if (yesPressed){
            PressButton(yesButton);
        }
        else{
            ReleaseButton(yesButton);
        }

        // if the no button is pressed
        if (noPressed){
            PressButton(noButton);
        }
        else{
            ReleaseButton(noButton);
        }
    }

    // toggles the button states when one is clicked (only one can be pressed down at a time)
    void ToggleButtonState(Button button)
    {
        // Determine which button was clicked and toggle both buttons states
        if (button == yesButton)
        {
            if (yesPressed)
            {
                ReleaseButton(yesButton);
            }
            else
            {
                PressButton(yesButton);
                if (noPressed)
                {
                    ReleaseButton(noButton);
                }
            }
        }
        else if (button == noButton)
        {
            if (noPressed)
            {
                ReleaseButton(noButton);
            }
            else
            {
                PressButton(noButton);
                if (yesPressed)
                {
                    ReleaseButton(yesButton);
                }
            }
        }

        // announce that the button states have been changed
        CallOnYesNoPressed();

    }

    // logic for pressing a button down
    void PressButton(Button button)
    {
        // Press the button and update its visual appearance
        if (button == yesButton)
        {
            yesPressed = true;
            yesButton.interactable = false;
        }
        else if (button == noButton)
        {
            noPressed = true;
            noButton.interactable = false;
        }
    }

    // logic for releasing a button
    void ReleaseButton(Button button)
    {
        // Release the button and update its visual appearance
        if (button == yesButton)
        {
            yesPressed = false;
            yesButton.interactable = true;
        }
        else if (button == noButton)
        {
            noPressed = false;
            noButton.interactable = true;
        }
    }

    // invoke the on yes and no pressed changed events
    void CallOnYesNoPressed(){
        if (OnYesPressedChanged != null){
            OnYesPressedChanged?.Invoke(yesPressed);
        }
        if (OnNoPressedChanged != null){
            OnNoPressedChanged?.Invoke(noPressed);
        }
    }
}
