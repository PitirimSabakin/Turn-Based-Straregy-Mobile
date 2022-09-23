using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBarImage; 
    private TMP_Text m_TextMeshPro;

    void Awake()
    {
        healthBarImage = transform.GetChild(0).GetComponent<Image>();
        m_TextMeshPro = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void NumberInHealthBar(float health)
    {
        m_TextMeshPro.text = health.ToString();
    }

    //When person take damage, health bar updates
    public void UpdateHealthBar(float health, float maxHealth)
    {
        healthBarImage.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1f);
        m_TextMeshPro.text = health.ToString();
    }

}
