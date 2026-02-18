using System.Linq;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float Weight;

    [HideInInspector] public bool IsPlaced = true;
    public bool IsBeingCarried => !IsPlaced;

    public Transform PickupPosition;
    [HideInInspector] public SpriteRenderer SpriteRenderer;

    [HideInInspector] public bool RequirementsSatisfied;

    new Rigidbody2D rigidbody;
    AutoSortOrder[] autoSortOrders;

    bool justDropped = false;
    float stuckTimer = 2.0f;
    public bool Stuck => justDropped && stuckTimer < 0.0f;

    GameObject warningFlash = null;
    GameObject stuckWarningFlash = null;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        autoSortOrders = GetComponentsInChildren<AutoSortOrder>(true);

        RequirementManager.AllItems.Add(this);
    }

    void Start()
    {
        CheckRequirements();
    }

    void OnDestroy()
    {
        RequirementManager.AllItems.Remove(this);
    }

    private void Update()
    {
        if (justDropped)
        {
            stuckTimer -= Time.deltaTime;

            if (Stuck && stuckWarningFlash == null)
            {
                stuckWarningFlash = Instantiate(Util.Instance.WarningIconRed, this.transform, false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (justDropped && rigidbody.IsSleeping())
        {
            justDropped = false;
            rigidbody.bodyType = RigidbodyType2D.Kinematic;

            if (stuckWarningFlash != null)
            {
                Destroy(stuckWarningFlash);
                stuckWarningFlash = null;
            }

            RequirementManager.CheckAllRequirements();
        }
    }

    public void OnPickup()
    {
        justDropped = false;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        rigidbody.linearVelocity = Vector2.zero;
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>(true))
        {
            if (collider.isTrigger == false)
                collider.enabled = false;
        }

        IsPlaced = false;

        foreach (AutoSortOrder order in autoSortOrders)
            order.YOffset = -1.5f;
        if (warningFlash != null)
        {
            Destroy(warningFlash);
            warningFlash = null;
        }
        if (stuckWarningFlash != null)
        {
            Destroy(stuckWarningFlash);
            stuckWarningFlash = null;
        }

        RequirementManager.CheckAllRequirements();
    }

    public void OnDrop()
    {
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>(true))
        {
            if (collider.isTrigger == false)
                collider.enabled = true;
        }

        IsPlaced = true;
        justDropped = true;
        stuckTimer = 2.0f;

        foreach (AutoSortOrder order in autoSortOrders)
            order.YOffset = 0.0f;
    }

    public void CheckRequirements()
    {
        if (IsBeingCarried || justDropped)
            return;

        bool all = true;
        foreach (Requirement req in GetComponentsInChildren<Requirement>())
        {
            if (req.IsSatisfied(this) == false)
            {
                all = false;
            }
        }

        RequirementsSatisfied = all;

        if (!RequirementsSatisfied && warningFlash == null)
        {
            warningFlash = Instantiate(Util.Instance.WarningIcon, this.transform, false);
        }
        else if (RequirementsSatisfied && warningFlash != null)
        {
            Destroy(warningFlash);
            warningFlash = null;
        }
    }
}
