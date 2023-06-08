using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_ZombleSpawn : MonoBehaviour
{
    [HideInInspector] public int numOfZombles;

    public GameObject zomble;
    public Vector2 minMaxTimeForSpawn;
    public Vector2 minMaxRangeForSpawn;
    public Vector2 chanceForSpawn = new Vector2(1, 1);
    public float timeBetweenDifficultyUpdate;
    public int maxAlive;
    public int numToSpawnEachTime;

    float spawnTime;

    void Update()
    {
        if (Time.time >= spawnTime)
        {
            if ((int)Random.Range(chanceForSpawn.x, chanceForSpawn.y) == 1)
            {
                for (int i = 0; i < numToSpawnEachTime; i++)
                {
                    if (numOfZombles < maxAlive)
                    {
                        GameObject instZomble = Instantiate(zomble, transform, true);
                        instZomble.transform.position = new Vector3(Random.Range(minMaxRangeForSpawn.x, minMaxRangeForSpawn.y), 0, Random.Range(minMaxRangeForSpawn.x, minMaxRangeForSpawn.y));
                        numOfZombles++;
                    }
                }
            }

            spawnTime = Random.Range(minMaxTimeForSpawn.x, minMaxTimeForSpawn.y) + spawnTime;
        }
    }
}
