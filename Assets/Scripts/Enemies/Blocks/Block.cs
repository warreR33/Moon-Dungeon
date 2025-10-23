using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Datos")]
    public BlockData data;

    [Header("Efectos")]
    [SerializeField] private string destructionEffectKey = "BlockDestruction";

    private int currentLife;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (data != null)
            InitializeFromData(data);
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
            OnDestroyed();
    }

    private void OnDestroyed()
    {
        Debug.Log($"Bloque destruido: {data.blockName} +{data.reward} puntos");

        if (data.spawnEnemyOnDestroy)
            Debug.Log($"El bloque {data.blockName} genera un enemigo.");

        // 🔥 Efecto de destrucción
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.SpawnFromPool(
                destructionEffectKey,
                transform.position,
                Quaternion.identity
            );
        }

        // ♻️ Devolver o destruir el bloque
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
