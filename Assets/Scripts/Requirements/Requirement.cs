using System;
using UnityEngine;

public abstract class Requirement : MonoBehaviour
{
    public abstract bool IsSatisfied(Item item);

    public string Description;
}
