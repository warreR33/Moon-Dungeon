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

    private ShipView view;
    private Camera mainCamera;


    [Header("Vida")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    [SerializeField] private ShipUI uiManager;

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
    }

    private void HandleMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        input = new Vector2(inputX, inputY).normalized;

        // Movimiento con aceleración
        if (input.magnitude > 0)
            velocity += input * acceleration * Time.deltaTime;
        else
            velocity = Vector2.Lerp(velocity, Vector2.zero, drag * Time.deltaTime);

        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        transform.position += (Vector3)velocity * Time.deltaTime;

        ClampToCamera();

        // Animaciones de la vista
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


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        // Aquí podríamos actualizar la UI de vida
        if (uiManager != null)
            uiManager.UpdateHealth(currentHealth);
    }
}
