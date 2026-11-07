using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 25;

    public float explosionRadius = 2f;
    public GameObject explosionPrefab;

    private GameObject owner;
    private bool exploded = false;

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

    private void Update()
    {
        transform.position +=
            transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded)
        {
            return;
        }

        CarHealth hitHealth =
            other.GetComponentInParent<CarHealth>();

        if (hitHealth != null &&
            hitHealth.gameObject == owner)
        {
            return;
        }

        // Trigger olan can paketleri gibi objeleri yok say
        if (other.isTrigger && hitHealth == null)
        {
            return;
        }

        Explode();
    }

    private void Explode()
    {
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

        System.Collections.Generic.HashSet<CarHealth>
            damagedCars =
            new System.Collections.Generic.HashSet<CarHealth>();

        foreach (Collider2D hit in hits)
        {
            CarHealth health =
                hit.GetComponentInParent<CarHealth>();

            if (health == null)
            {
                continue;
            }

            if (health.gameObject == owner)
            {
                continue;
            }

            // Bir arabada birden fazla Collider olsa bile
            // yalnızca bir kez hasar ver
            if (damagedCars.Add(health))
            {
                health.TakeDamage(damage);
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