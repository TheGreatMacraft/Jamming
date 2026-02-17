using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RequirementManager : MonoBehaviour
{
    public RequirementManager Instance;

    public static List<Item> AllItems = new();

    private void Awake()
    {
        Instance = this;
    }

    public static void CheckAllRequirements()
    {
        foreach (Item item in AllItems)
        {
            item.CheckRequirements();
        }
    }

    /*
    public bool AreAllRequirementsSatisfied()
    {
        Item[] items = FindObjectsByType<Item>(FindObjectsSortMode.None);
        return items.All(item => item.AreRequirementsSatisfied());
    }
    */
}
