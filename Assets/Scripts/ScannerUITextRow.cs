using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScannerUITextRow : MonoBehaviour
{

    public TMP_Text header;
    public TMP_Text body;

    public string headerText = "";
    public string bodyText = "";

    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    public void SetTextValues(string headerText, string bodyText)
    {
        this.headerText = headerText;
        this.bodyText = bodyText;

        SetText();
    }

    private void SetText(){
        if (header != null){
            header.text = headerText;
        }
        else{
            Debug.LogError("Header component is not assigned");
        }
        if (body != null){
            body.text = bodyText;
        }
        else{
            Debug.LogError("Body component is not assigned");
        }
    }
}
