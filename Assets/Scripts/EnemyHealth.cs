using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;

    public Action<GameObject> onEnemyDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        onEnemyDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }
}