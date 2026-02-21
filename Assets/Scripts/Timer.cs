using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public int gameMinutes;

    private int gameSecondsLeft;
    
    private float timer;

    private void Awake()
    {
        //gameSecondsLeft = gameMinutes * 60;
        gameSecondsLeft = 40;
        UpdateTimer();
    }

    private void Update()
    {
        if (gameSecondsLeft <= 0)
        {
            GameOverSequence.Instance.GameOver();
            print("Game Over");
            this.gameObject.SetActive(false);
        }
        
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            gameSecondsLeft--;
            timer = 0f;
            
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        int minutes = gameSecondsLeft / 60;
        int seconds = gameSecondsLeft - minutes * 60;
        
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
