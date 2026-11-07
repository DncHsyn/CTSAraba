using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;
    public Transform[] spawnPoints;

    public float respawnDelay = 10f;

    private GameObject[] activePickups;

    private void Start()
    {
        activePickups =
            new GameObject[spawnPoints.Length];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            StartCoroutine(PickupRoutine(i));
        }
    }

    private IEnumerator PickupRoutine(int index)
    {
        while (true)
        {
            if (spawnPoints[index] == null)
            {
                yield break;
            }

            activePickups[index] = Instantiate(
                pickupPrefab,
                spawnPoints[index].position,
                spawnPoints[index].rotation
            );

            // Paket toplanana kadar bekle
            yield return new WaitUntil(
                () => activePickups[index] == null
            );

            // Yeni paket için bekle
            yield return new WaitForSeconds(respawnDelay);
        }
    }
}