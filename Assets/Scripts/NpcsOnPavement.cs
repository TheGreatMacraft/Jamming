using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class NpcsOnPavement : MonoBehaviour
{
    public int timeUntilNpc;
    public float npcSpeed;
    
    [SerializeField] private RuntimeAnimatorController[] animationControllers;
    
    Random rnd = new Random();

    public Transform leftSide;
    public Transform rightSide;
    public GameObject npc;

    private void Start()
    {
        StartCoroutine(SpawnNpcAfterDelay(true));
        StartCoroutine(SpawnNpcAfterDelay(false));
    }

    private IEnumerator SpawnNpcAfterDelay(bool spawnOnLeft, float delay = 0.0f)
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);
        
        SpawnNpc(spawnOnLeft);
        
        float spawnAfterTime = rnd.Next(timeUntilNpc/2,timeUntilNpc);
        
        yield return SpawnNpcAfterDelay(spawnOnLeft, spawnAfterTime);
    }

    private void SpawnNpc(bool spawnOnLeft)
    {
        Transform npcSpawnpoint = spawnOnLeft ? leftSide : rightSide;
        
        GameObject newNpc = Instantiate(npc, npcSpawnpoint.position, npcSpawnpoint.rotation);
        
        int spriteIndex = rnd.Next(animationControllers.Length);
        newNpc.transform.Find("Model").GetComponent<Animator>().runtimeAnimatorController = animationControllers[spriteIndex];
        
        if (spawnOnLeft)
        {
            newNpc.GetComponent<Rigidbody2D>().AddForce(transform.right * npcSpeed);
        }
        else
        {
            newNpc.GetComponent<Rigidbody2D>().AddForce(-transform.right * npcSpeed);
            
            Vector3 scale = newNpc.transform.localScale;
            scale.x *= -1;
            newNpc.transform.localScale = scale;
        }
    }
}
