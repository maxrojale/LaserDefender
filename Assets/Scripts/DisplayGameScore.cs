using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayGameScore : MonoBehaviour
{
    GameSession gameSession;
    TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();    
    }

    // Update is called once per frame
    void Update()
    {
        int score = gameSession.GetScore();
        scoreText.text = score.ToString();
    }
}
