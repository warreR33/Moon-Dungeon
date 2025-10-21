using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float acceleration = 10f;     // Fuerza aplicada al moverse
    [SerializeField] private float maxSpeed = 8f;          // Velocidad máxima
    [SerializeField] private float drag = 5f;              // Freno natural (resistencia)

    [Header("Rotación")]
    [SerializeField] private float rotationSpeed = 720f;   

    [Header("Animación")]
    [SerializeField] private Animator animator;

    private Vector2 velocity;

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleTiltAnimation();
    }

    private Vector2 input;
    
    private void HandleMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        input = new Vector2(inputX, inputY).normalized;

        if (input.magnitude > 0)
        {
            velocity += input * acceleration * Time.deltaTime;
        }
        else
        {
            velocity = Vector2.Lerp(velocity, Vector2.zero, drag * Time.deltaTime);
        }

        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    private void HandleTiltAnimation()
    {
        Vector2 localInput = transform.InverseTransformDirection(input);

        float tilt = Mathf.Clamp(localInput.x, -1f, 1f);

        // Pasamos el valor al Animator
        animator.SetFloat("Tilt", tilt);
    }
}
