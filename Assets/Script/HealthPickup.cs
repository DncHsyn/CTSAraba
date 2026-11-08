
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 25;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
        {
            return;
        }

        CarHealth health =
            other.GetComponentInParent<CarHealth>();

        if (health == null)
        {
            return;
        }

        // Yalnızca Player can paketini toplayabilir.
        if (!health.CompareTag("Player"))
        {
            return;
        }

        // Canı zaten doluysa paket toplanmaz.
        if (health.currentHealth >= health.maxHealth)
        {
            return;
        }

        collected = true;

        health.Heal(healAmount);

        Destroy(gameObject);
    }
}
