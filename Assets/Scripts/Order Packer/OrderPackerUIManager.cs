using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderPackerUIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the Text component
    public TextMeshProUGUI countText; // Reference to the Text component
    private float startTime;
    private bool timerActive = false;

    void Start(){
        if(timerText == null){
            Debug.LogWarning("No Timer TMP Loaded");
        }
        if(countText == null){
            Debug.LogWarning("No Counter TMP Loaded");
        }
    }

    void Update()
    {
        if (timerText != null && timerActive)
        {
            float t = Time.time - startTime;
            t = Mathf.FloorToInt(t);
            string minutes = ((int) t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;
        }
    }

    public void StartTimer(){
        timerActive = true;
        startTime = Time.time;
    }

    // Call this to stop the timer
    public void StopTimer()
    {
        timerActive = false;
    }

    public void AddTime(float seconds){
        startTime += seconds;
    }

    //
    public void SetCountText(string countText){
        if(this.countText != null){
            this.countText.text = countText;
        }
    }
}