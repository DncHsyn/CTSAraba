using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 25;

    private GameObject owner;

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }

    void Update()
    {
        transform.position +=
            transform.up * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Roket kendi sahibine hasar vermesin
        if (collision.gameObject == owner)
        {
            return;
        }

        CarHealth carHealth =
            collision.GetComponentInParent<CarHealth>();

        if (carHealth != null)
        {
            // Aynı arabanın alt objelerini de kontrol et
            if (carHealth.gameObject == owner)
            {
                return;
            }

            carHealth.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}