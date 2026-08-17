using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLogic : MonoBehaviour
{
    
    public GameObject[] SPs;              // your 9 spawn points
    public GameObject[] EnemyPrefabs;     // your 4 enemy types

    public float delayBetweenSpawns = 0.3f;

    // ---------- Random spawn point picker (reusable) ----------
    public GameObject[] GetRandomSpawnPoints(GameObject[] sourcePoints, int count)
    {
        List<GameObject> pool = new List<GameObject>(sourcePoints);
        List<GameObject> selected = new List<GameObject>();

        count = Mathf.Min(count, pool.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            selected.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex);
        }

        return selected.ToArray();
    }

    // ---------- Spawns a wave: list of (enemy type index, count) pairs ----------
    public void SpawnWave(int[] enemyCountsPerType)
    {
        // enemyCountsPerType[0] = how many of EnemyPrefabs[0] to spawn, etc.
        int totalEnemies = 0;
        foreach (int c in enemyCountsPerType) totalEnemies += c;

        GameObject[] spawnPointsForWave = GetRandomSpawnPoints(SPs, totalEnemies);

        StartCoroutine(SpawnRoutine(enemyCountsPerType, spawnPointsForWave));
    }

    private IEnumerator SpawnRoutine(int[] enemyCountsPerType, GameObject[] spawnPoints)
    {
        int spawnIndex = 0;

        for (int typeIndex = 0; typeIndex < enemyCountsPerType.Length; typeIndex++)
        {
            for (int i = 0; i < enemyCountsPerType[typeIndex]; i++)
            {
                if (spawnIndex >= spawnPoints.Length) yield break; // safety, ran out of points

                GameObject spawnPoint = spawnPoints[spawnIndex];
                Instantiate(EnemyPrefabs[typeIndex], spawnPoint.transform.position, spawnPoint.transform.rotation);

                spawnIndex++;
                yield return new WaitForSeconds(delayBetweenSpawns);
            }
        }
    }
}
