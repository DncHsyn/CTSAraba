using UnityEngine;

public class BotHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private SpriteRenderer spriteRenderer;

    public Color fullHealthColor = new Color(1f, 0.6f, 0.6f);
    public Color lowHealthColor = new Color(0.4f, 0f, 0f);

    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateColor();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateColor();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void UpdateColor()
    {
        float healthPercent = (float)currentHealth / maxHealth;

        spriteRenderer.color = Color.Lerp(
            lowHealthColor,
            fullHealthColor,
            healthPercent
        );
    }
}