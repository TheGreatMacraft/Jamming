using Unity.VisualScripting;
using UnityEngine;

public class DeleteTag : MonoBehaviour
{

    public string tagToDelete;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagToDelete))
        {
            Destroy(other.gameObject);
        }
    }
}