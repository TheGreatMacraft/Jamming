using TMPro;
using UnityEngine;

public class BusinessMan : MonoBehaviour
{
    public static BusinessMan Instance;

    public string TalkPlayerGuideText;
    public string RequirementsNotOkText;
    public ItemGroup[] ItemsToGive;
    int itemGroupIndex = 0;
    int prevItemGroupIndex = -1;
    int itemGroupTotalCount = 0;

    public TMP_Text ItemTodoText;

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

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        SpeachText.text = "";
        ItemTodoText.text = TalkPlayerGuideText;
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
                
                SoundManager.PlaySound(SoundType.BUSINESSMAN_TALKING, 1f);
                
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
        while (itemGroupIndex < ItemsToGive.Length && ItemsToGive[itemGroupIndex].Count == 0)
            itemGroupIndex++;

        if (itemGroupIndex >= ItemsToGive.Length)
            return null;

        if (itemGroupIndex > prevItemGroupIndex && RequirementManager.AllRequirementsSatisfied == false)
        {
            ItemTodoText.text = RequirementsNotOkText;
            return null;
        }

        ItemGroup itemGroup = ItemsToGive[itemGroupIndex];
        GameObject go = Instantiate(itemGroup.ItemPrefab, transform.position, Quaternion.identity);

        if (itemGroupIndex > prevItemGroupIndex)
            itemGroupTotalCount = itemGroup.Count;

        itemGroup.Count -= 1;

        if (itemGroupTotalCount > 1)
        {
            int num = itemGroupTotalCount - itemGroup.Count;
            ItemTodoText.text = $"{itemGroup.UIText} ({num}/{itemGroupTotalCount})";
        }
        else
        {
            ItemTodoText.text = itemGroup.UIText;
        }

        prevItemGroupIndex = itemGroupIndex;

        return go.GetComponent<Item>();
    }

    public void OnPlayerItemDrop()
    {
        if (prevItemGroupIndex >= ItemsToGive.Length || prevItemGroupIndex < 0)
            return;

        if (ItemsToGive[itemGroupIndex].Count == 0 && RequirementManager.AllRequirementsSatisfied)
            ItemTodoText.text = TalkPlayerGuideText;
    }
}


[System.Serializable]
public class ItemGroup
{
    public GameObject ItemPrefab;
    public int Count;
    public string UIText;
}
