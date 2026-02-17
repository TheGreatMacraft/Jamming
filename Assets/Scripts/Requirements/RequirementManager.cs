using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RequirementManager : MonoBehaviour
{
    public RequirementManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    /*
    public bool AreAllRequirementsSatisfied()
    {
        Item[] items = FindObjectsByType<Item>(FindObjectsSortMode.None);
        return items.All(item => item.AreRequirementsSatisfied());
    }
    */
}
