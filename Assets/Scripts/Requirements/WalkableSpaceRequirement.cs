using System.Collections.Generic;
using UnityEngine;

public class WalkableSpaceRequirement : Requirement
{
    Collider2D spaceNeededCollider;

    List<Collider2D> queryResults = new();

    private void Awake()
    {
        spaceNeededCollider = GetComponent<Collider2D>();
    }

    public override bool IsSatisfied(Item item)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;

        queryResults.Clear();
        int count = Physics2D.OverlapCollider(spaceNeededCollider, contactFilter, queryResults);

        foreach (Collider2D collider in queryResults)
        {
            if (collider.CompareTag("Player") || collider.transform.parent?.CompareTag("Player") == true)
                continue;
            return false;
        }
        return true;
    }
}
