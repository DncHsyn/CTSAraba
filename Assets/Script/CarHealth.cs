
using UnityEngine;

public class CarHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color fullHealthColor = Color.white;
    public Color lowHealthColor = Color.red;

    [Header("Bot Respawn")]
    public BotSpawner spawner;
    public Transform mySpawnPoint;

    private bool isDead = false;
    private CameraShake cameraShake;

    private void Start()
    {
        currentHealth = maxHealth;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (CompareTag("Player"))
        {
            cameraShake = FindFirstObjectByType<CameraShake>();
        }

        UpdateColor();
    }

    public void TakeDamage(int damage, GameObject attacker = null)
    {
        if (isDead || damage <= 0)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateColor();

        // Kamera yalnızca Player hasar aldığında sallanır.
        if (CompareTag("Player") && cameraShake != null)
        {
            cameraShake.Shake();
        }

        if (currentHealth <= 0)
        {
            Die(attacker);
        }
    }

    public void Heal(int amount)
    {
        if (isDead || amount <= 0)
        {
            return;
        }

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateColor();
    }

    private void UpdateColor()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        float healthPercentage =
            (float)currentHealth / maxHealth;

        spriteRenderer.color = Color.Lerp(
            lowHealthColor,
            fullHealthColor,
            healthPercentage
        );
    }

    private void Die(GameObject attacker)
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        // Yalnızca Player'ın öldürdüğü Bot skor kazandırır.
        if (CompareTag("Bot") &&
            attacker != null &&
            attacker.CompareTag("Player"))
        {
            ScoreManager scoreManager =
                FindFirstObjectByType<ScoreManager>();

            if (scoreManager != null)
            {
                scoreManager.AddKill();
            }
        }

        // Bot kendi Spawn Point'inden yeniden doğar.
        if (CompareTag("Bot") &&
            spawner != null &&
            mySpawnPoint != null)
        {
            spawner.RespawnBot(mySpawnPoint);
        }

        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
