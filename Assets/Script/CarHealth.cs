using UnityEngine;

public class CarHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private SpriteRenderer spriteRenderer;
    public CameraShake cameraShake; // CameraShake scriptini buraya ekleyin

    public Color fullHealthColor =
        new Color(1f, 0.6f, 0.6f);

    public Color lowHealthColor =
        new Color(0.4f, 0f, 0f);

    [HideInInspector]
    public BotSpawner spawner;

    [HideInInspector]
    public Transform mySpawnPoint;

    private bool isDead = false;

    void Start()
{
    currentHealth = maxHealth;

    spriteRenderer = GetComponent<SpriteRenderer>();

    UpdateColor();

    if (CompareTag("Player"))
    {
        cameraShake = FindFirstObjectByType<CameraShake>();
    }
}

  public void TakeDamage(int damage)
{
    if (isDead || damage <= 0)
    {
        return;
    }

    currentHealth -= damage;

    currentHealth = Mathf.Max(currentHealth, 0);

    UpdateColor();

    // Sadece Player hasar aldığında kamera sallansın
    if (CompareTag("Player") && cameraShake != null)
    {
        cameraShake.Shake();
    }

    if (currentHealth <= 0)
    {
        Die();
    }
}

    void UpdateColor()
    {
        float healthPercent =
            (float)currentHealth / maxHealth;

        spriteRenderer.color = Color.Lerp(
            lowHealthColor,
            fullHealthColor,
            healthPercent
        );
    }

    void Die()
    {
        isDead = true;

        if (spawner != null && mySpawnPoint != null)
        {
            spawner.RespawnBot(mySpawnPoint);
        }

        Destroy(gameObject);
    }

    public void Heal(int amount)
{
    if (isDead || amount <= 0)
    {
        return;
    }

    currentHealth += amount;

    currentHealth = Mathf.Clamp(
        currentHealth,
        0,
        maxHealth
    );

    UpdateColor();
}
}