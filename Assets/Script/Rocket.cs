
using UnityEngine;
using System.Collections.Generic;

public class Rocket : MonoBehaviour
{
    [Header("Rocket Settings")]
    public float speed = 10f;
    public int damage = 25;
    public float lifeTime = 5f;

    [Header("Explosion Settings")]
    public float explosionRadius = 2f;
    public GameObject explosionPrefab;

    private Rigidbody2D rb;
    private GameObject owner;
    private bool exploded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (exploded)
        {
            return;
        }

        if (rb != null)
        {
            rb.MovePosition(
                rb.position +
                (Vector2)transform.up * speed * Time.fixedDeltaTime
            );
        }
        else
        {
            transform.position +=
                transform.up * speed * Time.fixedDeltaTime;
        }
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;

        if (owner == null)
        {
            return;
        }

        Collider2D[] ownerColliders =
            owner.GetComponentsInChildren<Collider2D>();

        Collider2D[] rocketColliders =
            GetComponentsInChildren<Collider2D>();

        foreach (Collider2D rocketCollider in rocketColliders)
        {
            foreach (Collider2D ownerCollider in ownerColliders)
            {
                Physics2D.IgnoreCollision(
                    rocketCollider,
                    ownerCollider
                );
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded)
        {
            return;
        }

        CarHealth hitHealth =
            other.GetComponentInParent<CarHealth>();

        // Roketin sahibine çarpmasını engelle.
        if (hitHealth != null &&
            hitHealth.gameObject == owner)
        {
            return;
        }

        // Can paketi gibi ilgisiz Trigger objelerini yok say.
        if (other.isTrigger && hitHealth == null)
        {
            return;
        }

        Explode();
    }

    private void Explode()
    {
        if (exploded)
        {
            return;
        }

        exploded = true;

        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius
        );

        HashSet<CarHealth> damagedCars =
            new HashSet<CarHealth>();

        foreach (Collider2D hit in hits)
        {
            CarHealth health =
                hit.GetComponentInParent<CarHealth>();

            if (health == null)
            {
                continue;
            }

            // Roketi atan araba kendi patlamasından hasar almaz.
            if (health.gameObject == owner)
            {
                continue;
            }

            // Aynı araba birden fazla kez hasar almasın.
            if (damagedCars.Add(health))
            {
                health.TakeDamage(damage, owner);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }
}
