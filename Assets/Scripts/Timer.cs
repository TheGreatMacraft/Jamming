using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    public TMP_Text timerText;

    public GameObject TimeIncreaseAnimationPrefab;

    public int gameMinutes;

    private int gameSecondsLeft;
    
    private float timer;

    public Color red_color;

    private void Awake()
    {
        gameSecondsLeft = gameMinutes * 60;
        //gameSecondsLeft = 40;

        Instance = this;
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
        
        else if (gameSecondsLeft == 60)
        {
            timerText.color = red_color;
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

    public void AddTime(int seconds)
    {
        if (seconds <= 0)
            return;

        gameSecondsLeft += seconds;
        UpdateTimer();

        GameObject popup = Instantiate(TimeIncreaseAnimationPrefab, transform);
        popup.GetComponent<RectTransform>().anchoredPosition = timerText.rectTransform.offsetMax;
        popup.GetComponent<TMP_Text>().text = $"+{seconds}s";
        Destroy(popup, 0.5f);
    }
}
