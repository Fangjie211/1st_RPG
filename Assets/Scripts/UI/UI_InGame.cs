using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;
    private SkillManager skills;


    [SerializeField] private TextMeshProUGUI currentSouls;
    private void Start()
    {
        UpdateHealthUI();
        if (playerStats != null)
        {
            playerStats.OnHealthChanged += UpdateHealthUI;
        }
        skills = SkillManager.instance;
    }

    private void Update()
    {
        currentSouls.text=PlayerManager.instance.CurrentCurrency().ToString();
        if (Input.GetKeyDown(KeyCode.LeftShift)&&skills.dash.dashUnlocked)
        {
            SetCooldownOf(dashImage);
        }
        if (Input.GetKeyDown(KeyCode.Q)&& skills.parry.parryUnlocked)
        {
            SetCooldownOf(parryImage);
        }
        if (Input.GetKeyDown(KeyCode.F)&&skills.crystal.crystalUnlocked)
        {
            SetCooldownOf(crystalImage);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)&&skills.sword.swordUnlocked)
        {
            SetCooldownOf(swordImage);
        }
        if (Input.GetKeyDown(KeyCode.R)&&skills.blackhole.blackholeUnlocked)
        {
            SetCooldownOf(blackholeImage);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)&&Inventory.instance.GetEquipment(EquipmentType.Flask)!=null)
        {
            SetCooldownOf(flaskImage);
        }
        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        CheckCooldownOf(swordImage,skills.sword.cooldown);
        CheckCooldownOf(blackholeImage,skills.blackhole.cooldown);
        //CheckCooldownOf(flaskImage, Inventory.instance.);
    }
    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealth();
        slider.value= playerStats.currentHealth;
        //Debug.Log(playerStats.currentHealth);
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
