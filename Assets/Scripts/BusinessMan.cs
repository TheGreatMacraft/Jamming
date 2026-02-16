using TMPro;
using UnityEngine;

public class BusinessMan : MonoBehaviour
{
    public GameObject[] ItemsToGive;
    int itemGiveIndex = 0;

    public TMP_Text SpeachText;
    public Vector2 QuietMinMaxTime;
    public float SecondsPerChar;
    public float SecondsBeforeQuiet;
    public string[] SpeachStrings;

    float speachTimer = 0.0f;
    bool speaking = false;

    string currentSpeachString;

    Animator animator;
    Player player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = FindFirstObjectByType<Player>();
        SpeachText.text = "";
    }

    private void Update()
    {
        if (speaking)
        {
            SpeachText.maxVisibleCharacters = Mathf.RoundToInt(speachTimer / SecondsPerChar);
            speachTimer += Time.deltaTime;

            if (speachTimer > SecondsPerChar * currentSpeachString.Length)
            {
                animator.Play("Idle");
            }

            if (speachTimer > SecondsPerChar * currentSpeachString.Length + SecondsBeforeQuiet)
            {
                speaking = false;
                SpeachText.text = "";
                animator.Play("Idle");
                speachTimer = Random.Range(QuietMinMaxTime.x, QuietMinMaxTime.y);
            }
        }
        else
        {
            speachTimer -= Time.deltaTime;

            if (speachTimer <= 0.0f && Vector2.Distance(player.transform.position, transform.position) < 6.0f)
            {
                string oldSpeachString = currentSpeachString;
                for (int i = 0; i < 20 && oldSpeachString == currentSpeachString; i++)
                {
                    currentSpeachString = SpeachStrings[Random.Range(0, SpeachStrings.Length)];
                }
                speaking = true;
                SpeachText.text = currentSpeachString;
                SpeachText.maxVisibleCharacters = 0;
                animator.Play("Talk");
                speachTimer = 0.0f;
            }
        }
    }

    public Item SpawnNewItem()
    {
        if (itemGiveIndex >= ItemsToGive.Length)
            return null;

        GameObject go = Instantiate(ItemsToGive[itemGiveIndex], transform.position, Quaternion.identity);
        itemGiveIndex++;

        return go.GetComponent<Item>();
    }
}
