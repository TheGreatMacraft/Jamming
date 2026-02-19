using UnityEngine;

public class Util : MonoBehaviour
{
    public GameObject WarningIcon;
    public GameObject WarningIconRed;

    public static Util Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static bool IsTagOnParent(GameObject gameObject, string tag)
    {
        if (gameObject.CompareTag(tag))
            return true;

        Transform t = gameObject.transform;
        while (t.parent != null)
        {
            t = t.parent;
            if (t.gameObject.CompareTag(tag))
                return true;
        }

        return false;
    }
}
