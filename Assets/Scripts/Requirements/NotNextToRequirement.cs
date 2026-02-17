using System.Collections.Generic;
using UnityEngine;

public class NotNextToRequirement : Requirement
{
    public string NotNextToTag;

    Collider2D reachCollider;

    List<Collider2D> queryResults = new();

    private void Awake()
    {
        reachCollider = GetComponent<Collider2D>();
    }

    public override bool IsSatisfied(Item item)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;

        queryResults.Clear();
        int count = Physics2D.OverlapCollider(reachCollider, contactFilter, queryResults);

        foreach (Collider2D collider in queryResults)
        {
            if (collider.CompareTag(NotNextToTag) || collider.transform.parent?.CompareTag(NotNextToTag) == true)
                return false;
        }
        return true;
    }
}
