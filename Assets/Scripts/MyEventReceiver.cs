using UnityEngine;

public class MyEventReceiver : MonoBehaviour
{
    private void PlaySound()
    {
        SoundManager.PlaySound(SoundType.FOOTSTEP);
    }
}