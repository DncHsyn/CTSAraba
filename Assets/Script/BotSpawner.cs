
using UnityEngine;
using System.Collections;

public class BotSpawner : MonoBehaviour
{
    [Header("Bot Settings")]
    public GameObject botPrefab;
    public Transform[] spawnPoints;

    [Header("Respawn Settings")]
    public float respawnDelay = 2f;

    private void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                SpawnBot(spawnPoint);
            }
        }
    }

    private void SpawnBot(Transform spawnPoint)
    {
        GameObject newBot = Instantiate(
            botPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        CarHealth health =
            newBot.GetComponent<CarHealth>();

        if (health != null)
        {
            health.spawner = this;
            health.mySpawnPoint = spawnPoint;
        }
        else
        {
            Debug.LogError(
                "Bot prefabında CarHealth scripti bulunamadı!"
            );
        }
    }

    public void RespawnBot(Transform spawnPoint)
    {
        if (spawnPoint == null)
        {
            return;
        }

        StartCoroutine(RespawnRoutine(spawnPoint));
    }

    private IEnumerator RespawnRoutine(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (spawnPoint != null)
        {
            SpawnBot(spawnPoint);
        }
    }
}
