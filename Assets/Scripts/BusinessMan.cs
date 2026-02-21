using TMPro;
using UnityEngine;

public class BusinessMan : MonoBehaviour
{
    public static BusinessMan Instance;

    public string TalkPlayerGuideText;
    public string RequirementsNotOkText;
    public ItemGroup[] ItemsToGive;
    int itemGroupIndex = -1;
    ItemGroup currentItemGroup = null;
    int itemGroupTotalCount = 0;

    public ItemGroup[] RandomlyPickedItems;

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

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
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

            if (speachTimer <= 0.0f && Vector2.Distance(Player.Instance.transform.position, transform.position) < 6.0f)
            {
                string oldSpeachString = currentSpeachString;
                for (int i = 0; i < 20 && oldSpeachString == currentSpeachString; i++)
                {
                    currentSpeachString = SpeachStrings[Random.Range(0, SpeachStrings.Length)];
                }
                
                SoundManager.PlaySound(SoundType.BUSINESSMAN_TALKING);
                
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
        // ce je treba najti nov ItemGroup
        if (currentItemGroup == null || currentItemGroup.Count == 0)
        {
            if (RequirementManager.AllRequirementsSatisfied == false)
            {
                ItemTodoText.text = RequirementsNotOkText;
                return null;
            }

            if (itemGroupIndex + 1 < ItemsToGive.Length)
            {
                itemGroupIndex++;
                currentItemGroup = ItemsToGive[itemGroupIndex];
            }
            else
            {
                currentItemGroup = RandomlyPickedItems[Random.Range(0, RandomlyPickedItems.Length)].Copy();
            }

            itemGroupTotalCount = currentItemGroup.Count;
        }

        GameObject go = Instantiate(currentItemGroup.ItemPrefab, transform.position, Quaternion.identity);
        currentItemGroup.Count -= 1;

        if (itemGroupTotalCount > 1)
        {
            int num = itemGroupTotalCount - currentItemGroup.Count;
            ItemTodoText.text = $"{currentItemGroup.UIText} ({num}/{itemGroupTotalCount})";
        }
        else
        {
            ItemTodoText.text = currentItemGroup.UIText;
        }

        return go.GetComponent<Item>();
    }

    public void OnPlayerItemDrop()
    {
        if (currentItemGroup == null)
            return;

        if (currentItemGroup.Count == 0 && RequirementManager.AllRequirementsSatisfied)
            ItemTodoText.text = TalkPlayerGuideText;
    }
}


[System.Serializable]
public class ItemGroup
{
    public GameObject ItemPrefab;
    public int Count;
    public string UIText;

    public ItemGroup Copy()
    {
        return (ItemGroup)MemberwiseClone();
    }
}
