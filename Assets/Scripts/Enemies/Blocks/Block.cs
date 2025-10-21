using UnityEngine;
using UnityEngine.UI; // si usás imágenes UI, si no, podés borrar esto

public class Block : MonoBehaviour
{
    [Header("Datos")]
    public BlockData data;

    private int currentLife;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (data != null)
        {
            InitializeFromData(data);
        }
    }

    public void InitializeFromData(BlockData blockData)
    {
        data = blockData;

        currentLife = data.life;

        if (spriteRenderer != null && data.sprite != null)
            spriteRenderer.sprite = data.sprite;
    }

    public void TakeDamage(int amount)
    {
        currentLife -= amount;

        if (currentLife <= 0)
        {
            OnDestroyed();
        }
    }

    private void OnDestroyed()
    {
        // Recompensa
        Debug.Log($"Bloque destruido: {data.blockName} +{data.reward} puntos");

        // Spawn enemigo si corresponde (lo implementaremos más adelante)
        if (data.spawnEnemyOnDestroy)
        {
            Debug.Log($"El bloque {data.blockName} genera un enemigo.");
        }

        Destroy(gameObject);
    }
}
