using UnityEngine;

public class Util : MonoBehaviour
{
    public static int CalcSortingOrder(float y)
    {
        return -Mathf.RoundToInt(y * 100.0f);
    }
}
