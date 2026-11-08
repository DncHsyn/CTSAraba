
using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
    [Header("Pickup Settings")]
    public GameObject pickupPrefab;
    public Transform[] spawnPoints;

    [Header("Respawn Settings")]
    public float respawnDelay = 10f;

    private GameObject[] activePickups;

    private void Start()
    {
        activePickups = new GameObject[spawnPoints.Length];

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

            // Paket yok edilene kadar bekle.
            yield return new WaitUntil(
                () => activePickups[index] == null
            );

            // Yeniden oluşmadan önce bekle.
            yield return new WaitForSeconds(respawnDelay);
        }
    }
}
