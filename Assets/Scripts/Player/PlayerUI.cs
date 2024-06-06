using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    public void Init(int startHealth)
    {
        healthBar.maxValue = startHealth;
        SetHealth(startHealth);
    }
    
    public void SetHealth(int health)
    {
        healthBar.value = health;
        healthText.text = health.ToString();
    }
}
