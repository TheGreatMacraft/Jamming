using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    public float Weight;

    [HideInInspector] public bool IsPlaced = true;
    public bool IsBeingCarried => !IsPlaced;
    public bool IsPlacedFixed => IsPlaced && !justDropped;

    public Transform PickupPosition;
    [HideInInspector] public SpriteRenderer SpriteRenderer;

    private bool _isClosestItem;
    public bool IsClosestItem
    {
        get => _isClosestItem;
        set {
            _isClosestItem = value;
            if (value == false) SpriteRenderer.color = Color.white;
        }
    }

    public bool RequirementsSatisfied { get; private set; }

    new Rigidbody2D rigidbody;
    AutoSortOrder[] autoSortOrders;
    Collider2D[] colliders;

    List<Collider2D> collisionTestResults = new();

    bool justDropped = false;
    float stuckTimer = 2.5f;
    public bool Stuck => justDropped && stuckTimer < 0.0f;

    GameObject warningFlash = null;
    GameObject stuckWarningFlash = null;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        autoSortOrders = GetComponentsInChildren<AutoSortOrder>(true);
        colliders = GetComponentsInChildren<Collider2D>(true);

        foreach (AutoSortOrder order in autoSortOrders)
            order.UpdateEveryFrame = false;

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

        if (IsClosestItem)
        {
            float darken = Mathf.Sin(Time.time * 4.0f) * 0.07f + 0.85f;
            SpriteRenderer.color = new Color(darken, darken, darken);
        }
    }

    private void FixedUpdate()
    {
        if (justDropped && rigidbody.IsSleeping() == false)
        {
            bool allClear = true;
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.useTriggers = false;

            foreach (Collider2D collider in colliders)
            {
                if (collider.isActiveAndEnabled == false)
                    continue;

                collisionTestResults.Clear();
                int count = Physics2D.OverlapCollider(collider, contactFilter, collisionTestResults);

                if (collisionTestResults.Any(col => colliders.Contains(col) == false))
                {
                    allClear = false;
                    break;
                }
            }

            if (allClear)
                rigidbody.Sleep();
        }

        if (justDropped && rigidbody.IsSleeping())
        {
            justDropped = false;
            rigidbody.bodyType = RigidbodyType2D.Kinematic;

            if (stuckWarningFlash != null)
            {
                Destroy(stuckWarningFlash);
                stuckWarningFlash = null;
            }

            foreach (AutoSortOrder order in autoSortOrders)
                order.UpdateEveryFrame = false;

            RequirementManager.CheckAllRequirements();
            BusinessMan.Instance.OnPlayerItemDrop();
        }
    }

    public void OnPickup()
    {
        justDropped = false;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        rigidbody.linearVelocity = Vector2.zero;
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger == false)
                collider.enabled = false;
        }

        IsPlaced = false;

        foreach (AutoSortOrder order in autoSortOrders)
        {
            order.YOffset -= 0.5f;
            order.UpdateEveryFrame = true;
        }
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
        RequirementManager.Instance.UpdateRequirementUI(GetComponentsInChildren<Requirement>());
    }

    public void OnDrop()
    {
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger == false)
                collider.enabled = true;
        }

        IsPlaced = true;
        justDropped = true;
        stuckTimer = 2.5f;

        foreach (AutoSortOrder order in autoSortOrders)
            order.YOffset += 0.5f;

        RequirementManager.Instance.ClearRequirementUI();
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

    public Vector2? ClosestPointOnCollider(Vector2 point)
    {
        if (IsBeingCarried)
            return null;

        return colliders
            .Where(collider => !collider.isTrigger && collider.isActiveAndEnabled)
            .Select(collider => collider.ClosestPoint(point))
            .OrderBy(pointOnCollider => (pointOnCollider - point).sqrMagnitude)
            .First();
    }
}
