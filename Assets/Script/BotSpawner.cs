using UnityEngine;
using System.Collections;

public class BotSpawner : MonoBehaviour
{
    public GameObject botPrefab;
    public Transform[] spawnPoints;

    public float respawnDelay = 3f;

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnBot(spawnPoints[i]);
        }
    }

    void SpawnBot(Transform spawnPoint)
    {
        GameObject newBot = Instantiate(
            botPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        CarHealth health = newBot.GetComponent<CarHealth>();

        if (health != null)
        {
            health.spawner = this;
            health.mySpawnPoint = spawnPoint;
        }
    }

    public void RespawnBot(Transform spawnPoint)
    {
        StartCoroutine(RespawnRoutine(spawnPoint));
    }

    IEnumerator RespawnRoutine(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnDelay);

        SpawnBot(spawnPoint);
    }
}