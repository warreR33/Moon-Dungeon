using UnityEngine;

[RequireComponent(typeof(ShipView))]
public class PlayerController : MonoBehaviour
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
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Colisión")]
    [SerializeField] private int collisionDamage = 10;
    [SerializeField] private float collisionCooldown = 1f;

    private float lastCollisionTime;
    private string lastColliderName;

    // ---------------------------------------------------------
    // Inicialización
    // ---------------------------------------------------------
    private void Awake()
    {
        view = GetComponent<ShipView>();
        mainCamera = Camera.main;

        currentHealth = maxHealth;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    // ---------------------------------------------------------
    // Movimiento y rotación
    // ---------------------------------------------------------
    private void HandleMovement()
    {
        // Input
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Movimiento
        if (input.magnitude > 0)
            velocity += input * acceleration * Time.deltaTime;
        else
            velocity = Vector2.Lerp(velocity, Vector2.zero, drag * Time.deltaTime);

        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        transform.position += (Vector3)velocity * Time.deltaTime;

        // Actualizar animaciones
        view.UpdateThrustAnimation(input.magnitude > 0);
        view.UpdateTiltAnimation(input, transform);
    }

    private void HandleRotation()
    {
        if (mainCamera == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // ---------------------------------------------------------
    // Colisiones
    // ---------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Boundary boundary))
        {
            TryTakeCollisionDamage(boundary.name);
        }
        else if (collision.collider.TryGetComponent(out Block block))
        {
            TryTakeCollisionDamage(block.name);
        }
    }

    private void TryTakeCollisionDamage(string sourceName)
    {
        if (sourceName == lastColliderName && Time.time - lastCollisionTime < collisionCooldown)
            return;

        TakeDamage(collisionDamage);

        lastColliderName = sourceName;
        lastCollisionTime = Time.time;
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Nave recibe {amount} de daño. Vida actual: {currentHealth}");
        view.PlayDamageFlash();
    }
}
