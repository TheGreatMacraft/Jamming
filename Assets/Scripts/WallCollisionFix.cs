using UnityEngine;

public class WallCollisionFix : MonoBehaviour
{
    public Collider2D ColliderExtendedUp;
    public Collider2D ColliderExtendedDown;
    public Collider2D ColliderDefault;
    public float YOffset;

    public bool Horizontal = false;

    public Bounds DefaultBounds { get; private set; }

    [HideInInspector] public bool ForceRefresh = true;

    bool playerAbove;
    Item item;

    private void Awake()
    {
        ColliderExtendedUp.enabled = false;
        ColliderExtendedDown.enabled = false;
        ColliderDefault.enabled = true;
        DefaultBounds = ColliderDefault.bounds;
        ForceRefresh = true;

        item = GetComponentInParent<Item>();
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
        if (item != null && !item.IsPlacedFixed)
        {
            ColliderExtendedUp.enabled = false;
            ColliderExtendedDown.enabled = false;
            ColliderDefault.enabled = true;
            DefaultBounds = ColliderDefault.bounds;
            ForceRefresh = true;
            return;
        }

        bool playerAboveNow;
        if (Horizontal)
            playerAboveNow = Player.Instance.transform.position.x > transform.position.x + YOffset;
        else
            playerAboveNow = Player.Instance.transform.position.y > transform.position.y + YOffset;

        if (ForceRefresh)
        {
            playerAbove = !playerAboveNow;
            ForceRefresh = false;
        }

        if (playerAboveNow && !playerAbove)
        {
            ColliderExtendedUp.enabled = false;
            ColliderExtendedDown.enabled = true;
            ColliderDefault.enabled = false;
        }
        else if (!playerAboveNow && playerAbove)
        {
            ColliderExtendedDown.enabled = false;
            ColliderExtendedUp.enabled = true;
            ColliderDefault.enabled = false;
        }

        playerAbove = playerAboveNow;
    }
}
