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
    AutoSortOrder autoSortOrder;

    bool justDropped = false;
    float stuckTimer = 1.0f;
    [HideInInspector] public bool Stuck = false;

    GameObject warningFlash = null;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        autoSortOrder = GetComponent<AutoSortOrder>();

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

            if (stuckTimer < 0.0f)
            {
                Stuck = true;
                // TODO: kaj s tem
                //Debug.Log("stuck " + gameObject.name);
            }
        }
    }

    private void FixedUpdate()
    {
        if (justDropped && rigidbody.IsSleeping())
        {
            justDropped = false;
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            RequirementManager.CheckAllRequirements();
        }
    }

    public void OnPickup()
    {
        justDropped = false;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        rigidbody.linearVelocity = Vector2.zero;
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.isTrigger == false)
                collider.enabled = false;
        }

        IsPlaced = false;

        autoSortOrder.YOffset = -1.5f;
        if (warningFlash != null)
        {
            Destroy(warningFlash);
            warningFlash = null;
        }

        RequirementManager.CheckAllRequirements();
    }

    public void OnDrop()
    {
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.isTrigger == false)
                collider.enabled = true;
        }

        IsPlaced = true;
        justDropped = true;

        autoSortOrder.YOffset = 0.0f;
    }

    public void CheckRequirements()
    {
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
