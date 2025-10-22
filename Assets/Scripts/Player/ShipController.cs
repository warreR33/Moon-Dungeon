using UnityEngine;

[RequireComponent(typeof(ShipView))]
public class ShipController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float drag = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    private Vector2 velocity;
    private Vector2 input;
    private Camera mainCamera;
    private ShipView view;

    [Header("Vida")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    [SerializeField] private ShipUI uiManager;

    [Header("Daño por borde")]
    [SerializeField] private int borderDamage = 1;
    [SerializeField] private float boundaryReleaseDistance = 0.02f;

    private BoundaryArea lastBoundaryTouched = null;
    private bool isTouchingBoundary = false;
    private float borderDamageTimer = 0f;

    // ---------------------------------------------------------
    // Ciclo de vida
    // ---------------------------------------------------------
    private void Awake()
    {
        view = GetComponent<ShipView>();
        mainCamera = Camera.main;

        currentHealth = maxHealth;
        uiManager?.InitHealth(maxHealth);
        uiManager?.UpdateHealth(currentHealth);
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();

        if (borderDamageTimer > 0f)
            borderDamageTimer -= Time.deltaTime;
    }

    // ---------------------------------------------------------
    // Movimiento y rotación
    // ---------------------------------------------------------
    private void HandleMovement()
    {
        // Input
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Movimiento básico
        if (input.magnitude > 0)
            velocity += input * acceleration * Time.deltaTime;
        else
            velocity = Vector2.Lerp(velocity, Vector2.zero, drag * Time.deltaTime);

        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        transform.position += (Vector3)velocity * Time.deltaTime;

        // Comprobación de límites
        HandleBoundaryCollision();

        // Mantener dentro de cámara
        ClampToCamera();

        // Actualizar animaciones
        view.UpdateThrustAnimation(input.magnitude > 0);
        view.UpdateTiltAnimation(input, transform);
    }

    private void HandleRotation()
    {
        if (mainCamera == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ClampToCamera()
    {
        Vector3 pos = transform.position;
        Vector3 viewPos = mainCamera.WorldToViewportPoint(pos);

        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);
        viewPos.y = Mathf.Clamp(viewPos.y, 0.05f, 0.95f);

        transform.position = mainCamera.ViewportToWorldPoint(viewPos);
    }

    // ---------------------------------------------------------
    // Colisión con bordes
    // ---------------------------------------------------------
    private void HandleBoundaryCollision()
    {
        if (BoundaryManager.Instance == null) return;

        Vector2 pos = transform.position;
        Vector2 vel = velocity;

        // Ajuste de posición
        Vector2 corrected = BoundaryManager.Instance.GetCorrectedPosition(pos, ref vel);
        transform.position = corrected;
        velocity = vel;

        // Detección de borde
        BoundaryArea touchedBoundary = BoundaryManager.Instance.GetBoundaryAtPosition(pos);

        if (touchedBoundary != null)
        {
            if (!isTouchingBoundary || touchedBoundary != lastBoundaryTouched)
            {
                TakeDamage(borderDamage);
                lastBoundaryTouched = touchedBoundary;
                isTouchingBoundary = true;
            }
        }
        else if (isTouchingBoundary)
        {
            if (lastBoundaryTouched != null)
            {
                float dist = Vector2.Distance(pos, lastBoundaryTouched.transform.position);
                if (dist > boundaryReleaseDistance)
                {
                    isTouchingBoundary = false;
                    lastBoundaryTouched = null;
                }
            }
            else
            {
                isTouchingBoundary = false;
            }
        }
    }

    // ---------------------------------------------------------
    // Daño y vida
    // ---------------------------------------------------------
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        uiManager?.UpdateHealth(currentHealth);
        view.PlayDamageFlash();
    }
}
