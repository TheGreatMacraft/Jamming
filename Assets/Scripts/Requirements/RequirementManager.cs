using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RequirementManager : MonoBehaviour
{
    public static RequirementManager Instance;
    public static List<Item> AllItems = new();

    public RectTransform RequirementUIParent;
    public RectTransform RequirementUIElement;

    private void Awake()
    {
        Instance = this;
        ClearRequirementUI();
    }

    public static void CheckAllRequirements()
    {
        foreach (Item item in AllItems)
        {
            item.CheckRequirements();
        }
    }

    public void ClearRequirementUI()
    {
        int count = RequirementUIParent.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            var child = RequirementUIParent.GetChild(i);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    public void UpdateRequirementUI(Requirement[] requirements)
    {
        ClearRequirementUI();

        foreach (Requirement requirement in requirements)
        {
            RectTransform element = Instantiate(RequirementUIElement, RequirementUIParent);
            TMP_Text text = element.Find("Description").GetComponent<TMP_Text>();
            text.text = requirement.Description;
        }
    }
}
