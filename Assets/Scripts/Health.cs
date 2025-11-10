using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public UnityEvent<float, float> onHealthChange;
    public UnityEvent onDeath;

    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChange?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        onHealthChange?.Invoke(currentHealth, maxHealth); // para UI
        Debug.Log($"Current health: {currentHealth}");
        if (currentHealth == 0) onDeath?.Invoke(); // para GameOver
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        var hitbox = other.GetComponent<Hitbox>();
        if (!hitbox) return;
        TakeDamage(hitbox.damage);
    }
}





