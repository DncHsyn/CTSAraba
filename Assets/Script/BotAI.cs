using UnityEngine;

public class BotAI : MonoBehaviour
{
    public float speed = 4f;
    public float rotationSpeed = 150f;

    public float detectionRange = 10f;
    public float shootingRange = 7f;
    public float fireCooldown = 2f;

    public GameObject rocketPrefab;
    public Transform firePoint;

    private Rigidbody2D rb;
    private Transform target;
    private float fireTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FindTarget();

        fireTimer -= Time.deltaTime;

        if (target != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                target.position
            );

            if (distance <= shootingRange &&
                fireTimer <= 0f)
            {
                FireRocket();
                fireTimer = fireCooldown;
            }
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            (target.position - transform.position).normalized;

        float targetAngle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg - 90f;

        float newAngle = Mathf.MoveTowardsAngle(
            rb.rotation,
            targetAngle,
            rotationSpeed * Time.fixedDeltaTime
        );

        rb.MoveRotation(newAngle);

        rb.linearVelocity = transform.up * speed;
    }

    void FindTarget()
    {
        GameObject[] players =
            GameObject.FindGameObjectsWithTag("Player");

        GameObject[] bots =
            GameObject.FindGameObjectsWithTag("Bot");

        Transform closestTarget = null;
        float closestDistance = detectionRange;

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(
                transform.position,
                player.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = player.transform;
            }
        }

        foreach (GameObject bot in bots)
        {
            if (bot == gameObject)
            {
                continue;
            }

            float distance = Vector2.Distance(
                transform.position,
                bot.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = bot.transform;
            }
        }

        target = closestTarget;
    }

   void FireRocket()
{
    if (rocketPrefab == null || firePoint == null)
    {
        return;
    }

    GameObject newRocket = Instantiate(
        rocketPrefab,
        firePoint.position,
        firePoint.rotation
    );

    Rocket rocketScript = newRocket.GetComponent<Rocket>();

    if (rocketScript != null)
    {
        rocketScript.SetOwner(gameObject);
    }
}
}