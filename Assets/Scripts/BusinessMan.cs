using UnityEngine;

public class BusinessMan : MonoBehaviour
{
    public GameObject[] ItemsToGive;
    int itemGiveIndex = 0;

    public Item SpawnNewItem()
    {
        if (itemGiveIndex >= ItemsToGive.Length)
            return null;

        GameObject go = Instantiate(ItemsToGive[itemGiveIndex], transform.position, Quaternion.identity);
        itemGiveIndex++;

        return go.GetComponent<Item>();
    }
}
