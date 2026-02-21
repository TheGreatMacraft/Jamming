using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform Target;
    public float Speed;

    public bool IsOnTarget;
    public float radiousToBeOnTarget;

    void Update()
    {
        Vector2 newPosition = Vector2.Lerp(transform.position, Target.position, Speed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        
        float distance = Vector2.Distance(transform.position, Target.position);
        IsOnTarget = distance <= radiousToBeOnTarget;
    }
}
