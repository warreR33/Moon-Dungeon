using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float drag = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Camera mainCamera;

    private ShipView view;
    private ShipModel model;

    [Header("Colisión")]
    [SerializeField] private int collisionDamage = 1;
    [SerializeField] private float collisionCooldown = 1f;

    private float lastCollisionTime;
    private string lastColliderName;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        mainCamera = Camera.main;
    }

    public void Initialize(ShipView view, ShipModel model)
    {
        this.view = view;
        this.model = model;
    }

    private void Update()
    {
        if (view == null || model == null) return;

        HandleInput();
        HandleRotation();
        view.UpdateTiltAnimation(input, transform);
    }

    private void FixedUpdate()
    {
        if (view == null || model == null) return;
        HandleMovement();
    }

    private void HandleInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        view.UpdateThrustAnimation(input.magnitude > 0);
    }

    private void HandleMovement()
    {
        if (input.magnitude > 0)
        {
            rb.AddForce(input * acceleration);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, drag * Time.fixedDeltaTime);
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    private void HandleRotation()
    {
        if (mainCamera == null) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 

        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        rb.rotation = angle - 90f;
    }

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

        model.TakeDamage(collisionDamage);
        view.PlayDamageFlash();

        lastColliderName = sourceName;
        lastCollisionTime = Time.time;
    }
}
