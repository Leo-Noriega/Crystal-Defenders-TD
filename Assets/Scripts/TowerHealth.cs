using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the tower
    private float currentHealth;

    public string towerName; // Unique name or ID for the tower
    private Slider healthSlider; // Reference to the UI Slider for health display

    void Start()
    {
        // Initialize the health
        currentHealth = maxHealth;

        // Find the corresponding health bar in the UI
        healthSlider = GameObject.Find(towerName + "HealthBar").GetComponent<Slider>();

        // Set the slider's max value and current value
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        // Reduce health by the damage amount
        currentHealth -= damage;

        // Update the health slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Check if the tower's health is depleted
        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

    void DestroyTower()
    {
        // Destroy the tower
        Destroy(gameObject);

        // Optionally, you can add effects or logic for when the tower is destroyed
        Debug.Log($"{gameObject.name} has been destroyed!");
    }
}