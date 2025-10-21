using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 2f;   // Velocidad de movimiento vertical
    [SerializeField] private float resetHeight = 20f;  // Altura del bloque (depende del tamaño del tilemap)
    [SerializeField] private bool scrollUp = true;     // Direccion del scroll (true = hacia arriba)

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Dirección del movimiento
        float direction = scrollUp ? 1f : -1f;

        // Mover
        transform.Translate(Vector3.up * direction * scrollSpeed * Time.deltaTime);

        // Calcular distancia recorrida
        float distance = Mathf.Abs(transform.position.y - startPos.y);

        // Cuando se pasa del límite, lo reseteamos
        if (distance >= resetHeight)
        {
            float offset = resetHeight * direction;
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - offset,
                transform.position.z
            );
        }
    }
}
