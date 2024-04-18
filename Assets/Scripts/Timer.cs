using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float startTime;

    private bool gameOver = false;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float t = Time.time - startTime;

        if (t < 300f)
        {
            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            timerText.text = minutes + ":" + seconds;
        }
        else
        {
            gameOver = true;
            // If elapsed time is 3 minutes or more, stop updating the timer text
            timerText.text = "05:00"; // Display 3 minutes
        }
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

}