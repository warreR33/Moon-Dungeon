using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 15f;

    [Header("Daño")]
    private float damage;
    public void SetDamage(float dmg) => damage = dmg;

    [Header("Impacto")]
    [SerializeField] private GameObject impactEffectPrefab;

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        ObjectPool.Instance.ReturnToPool(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Block block = collision.GetComponent<Block>();
        if (block != null)
        {
            block.TakeDamage(Mathf.CeilToInt(damage));
        }
        if (impactEffectPrefab != null)
        {
            ObjectPool.Instance.SpawnFromPool("Impact", transform.position, Quaternion.identity);
        }
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
