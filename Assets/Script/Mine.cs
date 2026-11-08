
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Mine : MonoBehaviour
{
    [Header("Mine Settings")]
    public int damage = 40;
    public float explosionRadius = 3f;
    public float armDelay = 0.5f;
    public float lifeTime = 30f;

    [Header("Explosion Effect")]
    public GameObject explosionPrefab;

    private GameObject owner;
    private bool isArmed;
    private bool hasExploded;

    private void Start()
    {
        // Mayının hemen patlamasını engeller.
        Invoke(nameof(ArmMine), armDelay);

        // Belirlenen süre sonunda mayını siler.
        Destroy(gameObject, lifeTime);
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }

    private void ArmMine()
    {
        isArmed = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isArmed || hasExploded)
            return;

        CarHealth targetHealth =
            other.GetComponentInParent<CarHealth>();

        // Çarpışılan nesne bir araç değilse işlem yapma.
        if (targetHealth == null)
            return;

        // Mayını bırakan araç kendi mayınıyla hasar almasın.
        if (targetHealth.gameObject == owner)
            return;

        Explode();
    }

    private void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        // Patlama görselini oluştur.
        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // Patlama alanındaki bütün Collider2D nesnelerini bul.
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius
        );

        // Aynı araca birden fazla kez hasar verilmesini engelle.
        HashSet<CarHealth> damagedCars =
            new HashSet<CarHealth>();

        foreach (Collider2D hit in hits)
        {
            CarHealth health =
                hit.GetComponentInParent<CarHealth>();

            if (health == null)
                continue;

            // Mayını bırakan aracı hasar listesinden çıkar.
            if (health.gameObject == owner)
                continue;

            // Aynı aracın birden fazla Collider'ı olabilir.
            if (damagedCars.Contains(health))
                continue;

            damagedCars.Add(health);

            health.TakeDamage(damage, owner);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }
}
