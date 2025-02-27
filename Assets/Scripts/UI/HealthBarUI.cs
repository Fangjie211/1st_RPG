using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    private Entity entity;
    private RectTransform myTransform;
    private CharacterStats myStats;
    private Slider slider;

    private void Start()
    {
        myStats = GetComponentInParent<CharacterStats>();
        slider= GetComponentInChildren<Slider>();
        myTransform = GetComponent<RectTransform>();
        entity= GetComponentInParent<Entity>();
        entity.onFlipped += FlipUI;
        myStats.OnHealthChanged += UpdateHealthUI;
        UpdateHealthUI();
    }
   
    private void UpdateHealthUI()
    {
        slider.maxValue=myStats.GetMaxHealth();
        slider.value = myStats.currentHealth;
    }
    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }
    private void OnDisable()
    {

        entity.onFlipped -= FlipUI;
        myStats.OnHealthChanged -= UpdateHealthUI;
    }    
}
