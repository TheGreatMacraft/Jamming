using System.Collections.Generic;
using UnityEngine;

public class NotNextToRequirement : Requirement
{
    public string NotNextToTag;
    public Collider2D ReachCollider;

    List<Collider2D> queryResults = new();

    public override bool IsSatisfied(Item item)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;

        queryResults.Clear();
        int count = Physics2D.OverlapCollider(ReachCollider, contactFilter, queryResults);

        foreach (Collider2D collider in queryResults)
        {
            if (collider.CompareTag(NotNextToTag) || collider.transform.parent?.CompareTag(NotNextToTag) == true)
                return false;
        }
        return true;
    }
}
