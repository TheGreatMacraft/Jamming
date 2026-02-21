using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text ScoreText;
    public GameObject ScorePopupPrefab;

    public static ScoreUI Instance;

    public int CurrentScore { get; private set; } = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ScoreText.text = $"$ {CurrentScore} $";
    }

    public void AddScore(int amount)
    {
        if (amount <= 0)
            return;
        CurrentScore += amount;
        ScoreText.text = $"$ {CurrentScore} $";

        ScoreText.ForceMeshUpdate();
        LayoutRebuilder.ForceRebuildLayoutImmediate(ScoreText.rectTransform);

        GameObject popup = Instantiate(ScorePopupPrefab, transform);
        popup.GetComponent<RectTransform>().anchoredPosition = ScoreText.rectTransform.offsetMax;
        popup.GetComponent<TMP_Text>().text = $"+{amount}$";
        Destroy(popup, 0.5f);
    }

    public void RemoveScore(int amount)
    {
        if (amount <= 0)
            return;
        CurrentScore -= amount;
        ScoreText.text = $"$ {CurrentScore} $";
    }
}
