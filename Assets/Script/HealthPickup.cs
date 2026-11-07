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

        if (!health.CompareTag("Player"))
        {
            return;
        }

        if (health.currentHealth <= 0 ||
            health.currentHealth >= health.maxHealth)
        {
            return;
        }

        collected = true;

        health.Heal(healAmount);

        Destroy(gameObject);
    }
}