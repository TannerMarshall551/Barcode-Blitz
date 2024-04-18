using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPScore : MonoBehaviour
{
    public GameObject rpGameManager;
    private ReturnsProcessorGameManager gameManager;
    int score;

    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = rpGameManager.GetComponent<ReturnsProcessorGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        score = gameManager.GetScore();
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        string scoreString = score.ToString();
        scoreText.text = "Score:" + scoreString + "/5"; // Modify as needed to format your score display
    }
}
