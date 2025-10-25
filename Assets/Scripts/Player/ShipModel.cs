using UnityEngine;
using System;

[System.Serializable]
public class ShipModel
{
    [Header("Estadísticas")]
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public event Action<int> OnHealthChanged;

    public void Initialize()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth); 
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Nave recibe {amount} de daño. Vida actual: {currentHealth}");

        OnHealthChanged?.Invoke(currentHealth); 
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    public bool IsDead => currentHealth <= 0;
}
