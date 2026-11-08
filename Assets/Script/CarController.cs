
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float rotationSpeed = 180f;

    [Header("Rocket")]
    public GameObject rocketPrefab;
    public Transform firePoint;

    [Header("Mine")]
    public GameObject minePrefab;
    public Transform mineDropPoint;
    public KeyCode mineKey = KeyCode.Q;
    public int startingMines = 3;
    public int maxMines = 10;
    public TMP_Text mineText;

    [Header("Nitro")]
    public float nitroMultiplier = 2f;
    public float maxNitro = 100f;
    public float nitroConsumption = 30f;
    public float nitroRecharge = 15f;
    public Slider nitroSlider;

    private Rigidbody2D rb;

    private float moveInput;
    private float turnInput;

    private float currentNitro;
    private int currentMines;

    private bool usingNitro;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentNitro = maxNitro;

        currentMines = Mathf.Clamp(
            startingMines,
            0,
            maxMines
        );

        if (nitroSlider != null)
        {
            nitroSlider.minValue = 0;
            nitroSlider.maxValue = maxNitro;
            nitroSlider.value = currentNitro;
        }

        UpdateMineUI();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        // E tuşuyla roket at.
        if (Input.GetKeyDown(KeyCode.E))
        {
            FireRocket();
        }

        // Q tuşuyla mayın bırak.
        if (Input.GetKeyDown(mineKey))
        {
            DropMine();
        }

        UpdateNitro();
    }

    private void FixedUpdate()
    {
        float currentSpeed = usingNitro
            ? speed * nitroMultiplier
            : speed;

        rb.linearVelocity =
            transform.up * moveInput * currentSpeed;

        float rotation =
            -turnInput * rotationSpeed * Time.fixedDeltaTime;

        rb.MoveRotation(rb.rotation + rotation);
    }

    private void FireRocket()
    {
        if (rocketPrefab == null || firePoint == null)
        {
            Debug.LogWarning(
                "Rocket Prefab veya Fire Point atanmamış!"
            );

            return;
        }

        GameObject rocketObject = Instantiate(
            rocketPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rocket rocket = rocketObject.GetComponent<Rocket>();

        if (rocket != null)
        {
            rocket.SetOwner(gameObject);
        }
    }

    private void DropMine()
    {
        if (minePrefab == null)
        {
            Debug.LogWarning("Mine Prefab atanmamış!");
            return;
        }

        if (currentMines <= 0)
        {
            Debug.Log("Mayın kalmadı!");
            return;
        }

        Vector3 dropPosition;

        if (mineDropPoint != null)
        {
            dropPosition = mineDropPoint.position;
        }
        else
        {
            dropPosition =
                transform.position - transform.up * 1.2f;
        }

        GameObject mineObject = Instantiate(
            minePrefab,
            dropPosition,
            Quaternion.identity
        );

        Mine mine = mineObject.GetComponent<Mine>();

        if (mine != null)
        {
            mine.SetOwner(gameObject);
        }
        else
        {
            Debug.LogError(
                "Mine prefabında Mine.cs bulunamadı!"
            );

            Destroy(mineObject);
            return;
        }

        currentMines--;

        UpdateMineUI();

        Debug.Log("Mayın bırakıldı. Kalan mayın: " + currentMines);
    }

    public void AddMines(int amount)
    {
        if (amount <= 0)
            return;

        currentMines = Mathf.Clamp(
            currentMines + amount,
            0,
            maxMines
        );

        UpdateMineUI();
    }

    private void UpdateMineUI()
    {
        if (mineText != null)
        {
            mineText.text = "MINE: " + currentMines;
        }
    }

    private void UpdateNitro()
    {
        usingNitro =
            Input.GetKey(KeyCode.LeftShift) &&
            currentNitro > 0 &&
            moveInput > 0;

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
            0,
            maxNitro
        );

        if (nitroSlider != null)
        {
            nitroSlider.value = currentNitro;
        }
    }
}
