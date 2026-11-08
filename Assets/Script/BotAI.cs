
using UnityEngine;

public class BotAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4f;
    public float rotationSpeed = 150f;

    [Header("Detection")]
    public float detectionRange = 10f;
    public float shootingRange = 7f;

    [Header("Shooting")]
    public GameObject rocketPrefab;
    public Transform firePoint;
    public float fireCooldown = 2f;

    private Rigidbody2D rb;
    private Transform target;

    private float nextFireTime;
    private float currentRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FindTarget();

        if (target != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                target.position
            );

            if (distance <= shootingRange &&
                Time.time >= nextFireTime)
            {
                FireRocket();
                nextFireTime = Time.time + fireCooldown;
            }
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            (Vector2)(target.position - transform.position);

        float targetAngle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg - 90f;

        currentRotation = Mathf.MoveTowardsAngle(
            rb.rotation,
            targetAngle,
            rotationSpeed * Time.fixedDeltaTime
        );

        rb.MoveRotation(currentRotation);

        rb.linearVelocity = transform.up * speed;
    }

    private void FindTarget()
    {
        target = null;

        float closestDistance = detectionRange;

        GameObject[] players =
            GameObject.FindGameObjectsWithTag("Player");

        GameObject[] bots =
            GameObject.FindGameObjectsWithTag("Bot");

        foreach (GameObject candidate in players)
        {
            CheckTarget(candidate, ref closestDistance);
        }

        foreach (GameObject candidate in bots)
        {
            if (candidate == gameObject)
            {
                continue;
            }

            CheckTarget(candidate, ref closestDistance);
        }
    }

    private void CheckTarget(
        GameObject candidate,
        ref float closestDistance)
    {
        if (candidate == null)
        {
            return;
        }

        CarHealth health =
            candidate.GetComponent<CarHealth>();

        if (health == null || health.IsDead())
        {
            return;
        }

        float distance = Vector2.Distance(
            transform.position,
            candidate.transform.position
        );

        if (distance < closestDistance)
        {
            closestDistance = distance;
            target = candidate.transform;
        }
    }

    private void FireRocket()
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

        Rocket rocket =
            newRocket.GetComponent<Rocket>();

        if (rocket != null)
        {
            rocket.SetOwner(gameObject);
        }
    }
}
