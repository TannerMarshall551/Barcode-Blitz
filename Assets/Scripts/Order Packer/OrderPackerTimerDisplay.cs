using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderPackerTimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the Text component
    private float startTime;
    private bool timerActive = false;

    void Update()
    {
        if (timerActive)
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
}