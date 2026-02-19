using UnityEngine;

public class WallCollisionFix : MonoBehaviour
{
    public Collider2D ColliderExtendedUp;
    public Collider2D ColliderExtendedDown;
    public Collider2D ColliderDefault;
    public float YOffset;

    public bool Horizontal = false;
    public bool OnlyDefault = false;

    bool playerAbove;
    Item item;

    private void Awake()
    {
        ColliderExtendedUp.enabled = false;
        ColliderExtendedDown.enabled = false;
        if (ColliderDefault != null) ColliderDefault.enabled = true;

        item = GetComponentInParent<Item>();
    }

    private void Start()
    {
        if (Horizontal)
            playerAbove = Player.Instance.transform.position.x <= transform.position.x + YOffset;
        else
            playerAbove = Player.Instance.transform.position.y <= transform.position.y + YOffset;
    }

    private void Update()
    {
        if (item != null && item.IsBeingCarried == true)
        {
            ColliderExtendedUp.enabled = false;
            ColliderExtendedDown.enabled = false;
            ColliderDefault.enabled = false;
            return;
        }
        if (OnlyDefault || (item != null && !item.IsPlacedFixed))
        {
            ColliderExtendedUp.enabled = false;
            ColliderExtendedDown.enabled = false;
            ColliderDefault.enabled = true;
            return;
        }

        bool playerAboveNow;
        if (Horizontal)
            playerAboveNow = Player.Instance.transform.position.x > transform.position.x + YOffset;
        else
            playerAboveNow = Player.Instance.transform.position.y > transform.position.y + YOffset;

        if (playerAboveNow && !playerAbove)
        {
            ColliderExtendedUp.enabled = false;
            ColliderExtendedDown.enabled = true;
            if (ColliderDefault != null) ColliderDefault.enabled = false;
        }
        else if (!playerAboveNow && playerAbove)
        {
            ColliderExtendedDown.enabled = false;
            ColliderExtendedUp.enabled = true;
            if (ColliderDefault != null) ColliderDefault.enabled = false;
        }

        playerAbove = playerAboveNow;
    }
}
