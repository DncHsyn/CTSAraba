using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 200f;

    public GameObject rocketPrefab;
    public Transform firePoint;

    private Rigidbody2D rb;

    private float moveInput;
    private float turnInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.E))
        {
            FireRocket();
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = transform.up * moveInput * speed;
        rb.linearVelocity = movement;

        float rotation = -turnInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotation);
    }

    void FireRocket()
{
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