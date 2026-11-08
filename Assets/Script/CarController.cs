
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float rotationSpeed = 180f;

    [Header("Rocket")]
    public GameObject rocketPrefab;
    public Transform firePoint;

    [Header("Nitro")]
    public float nitroSpeedMultiplier = 2f;
    public float maxNitro = 100f;
    public float nitroConsumption = 30f;
    public float nitroRecharge = 15f;

    [Header("Nitro UI")]
    public Slider nitroSlider;

    private Rigidbody2D rb;

    private float moveInput;
    private float turnInput;

    private float currentNitro;
    private bool usingNitro;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentNitro = maxNitro;

        if (nitroSlider != null)
        {
            nitroSlider.maxValue = maxNitro;
            nitroSlider.value = currentNitro;
        }
    }

    private void Update()
    {
        // WASD hareket girdileri.
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        // E tuşu ile roket at.
        if (Input.GetKeyDown(KeyCode.E))
        {
            FireRocket();
        }

        UpdateNitro();
    }

    private void FixedUpdate()
    {
        float currentSpeed = speed;

        // Nitro aktifse hız çarpanını uygula.
        if (usingNitro)
        {
            currentSpeed *= nitroSpeedMultiplier;
        }

        rb.linearVelocity =
            transform.up * moveInput * currentSpeed;

        rb.MoveRotation(
            rb.rotation -
            turnInput * rotationSpeed * Time.fixedDeltaTime
        );
    }

    private void UpdateNitro()
    {
        bool shiftPressed =
            Input.GetKey(KeyCode.LeftShift);

        // Nitro sadece ileri giderken kullanılabilir.
        usingNitro =
            shiftPressed &&
            currentNitro > 0f &&
            moveInput > 0f;

        if (usingNitro)
        {
            currentNitro -=
                nitroConsumption * Time.deltaTime;
        }
        else
        {
            currentNitro +=
                nitroRecharge * Time.deltaTime;
        }

        currentNitro = Mathf.Clamp(
            currentNitro,
            0f,
            maxNitro
        );

        if (currentNitro <= 0f)
        {
            usingNitro = false;
        }

        UpdateNitroUI();
    }

    private void UpdateNitroUI()
    {
        if (nitroSlider == null)
        {
            return;
        }

        nitroSlider.maxValue = maxNitro;
        nitroSlider.value = currentNitro;
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
