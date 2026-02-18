using Unity.VisualScripting;
using UnityEngine;

public class DeleteCar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("car"))
        {
            Destroy(other.gameObject);
        }
    }
}