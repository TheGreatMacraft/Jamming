using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class CarsOnRoad : MonoBehaviour
{
    public int timeUntilCar;
    public float carSpeed;

    [SerializeField] private RuntimeAnimatorController[] animationControllers;
    
    Random rnd = new Random();

    public Transform leftSide;
    public Transform rightSide;
    public GameObject car;

    private void Start()
    {
        StartCoroutine(SpawnCarAfterDelay(true));
        StartCoroutine(SpawnCarAfterDelay(false));
    }

    private IEnumerator SpawnCarAfterDelay(bool spawnOnLeft, float delay = 0.0f)
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);
        
        SpawnCar(spawnOnLeft);
        
        float spawnAfterTime = rnd.Next(timeUntilCar/8, timeUntilCar);
        
        yield return SpawnCarAfterDelay(spawnOnLeft, spawnAfterTime);
    }

    private void SpawnCar(bool spawnOnLeft)
    {
        Transform carSpawnpoint = spawnOnLeft ? leftSide : rightSide;
        
        GameObject newCar = Instantiate(car, carSpawnpoint.position, carSpawnpoint.rotation);

        int spriteIndex = rnd.Next(animationControllers.Length);
        newCar.transform.Find("Model").GetComponent<Animator>().runtimeAnimatorController = animationControllers[spriteIndex];
        
        if (spawnOnLeft)
        {
            newCar.GetComponent<Rigidbody2D>().AddForce(transform.right * carSpeed);
        }
        else
        {
            newCar.GetComponent<Rigidbody2D>().AddForce(-transform.right * carSpeed);
            
            Vector3 scale = newCar.transform.localScale;
            scale.x *= -1;
            newCar.transform.localScale = scale;
        }
    }
}
