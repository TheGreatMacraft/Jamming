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
}
