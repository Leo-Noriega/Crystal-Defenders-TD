using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private string label; // Texto que aparece como "Current Health"
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Image healthBar;
    [SerializeField] private RectTransform heartIconContainer;
    [SerializeField] private GameObject heartIconPrefab;
    private List<GameObject> heartIcons = new();

    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            var instance = Instantiate(heartIconPrefab, heartIconContainer);
            heartIcons.Add(instance);
            instance.SetActive(false);
        }
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        textComponent.text = $"{label}: {health}";
        var normalizedHealth = health / maxHealth;
        healthBar.fillAmount = normalizedHealth;
        var hearts = (int)(health / 10);
        for (var i = 0; i < heartIcons.Count; i++) heartIcons[i].SetActive(i < hearts);
    }
}
