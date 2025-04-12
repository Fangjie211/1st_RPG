using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,ISavemanager
{

    private UI ui;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedColor;
    public bool unlocked;
    [SerializeField] private int price;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    private Image skillImage;


    private void OnValidate()
    {
        gameObject.name="SkillTreeSlot_UI - "+skillName;
    }
    private void Awake()
    {
        
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }
    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedColor;
        ui= GetComponentInParent<UI>();
        if (unlocked)
        {
            skillImage.color = Color.white;
        }

    }
    public void UnlockSkillSlot()
    {
        if (unlocked || !PlayerManager.instance.HaveEnoughMoney(price))
        {
            return;
        }
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("cannot unlock this skill yet");
                return;
            }
        }
        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("has unlocked another item");
                return;
            }
        }
        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltip.ShowToolTip(skillDescription,skillName);
        //Vector2 mousePosition = Input.mousePosition;
        //float xOffset = 0;
        //if (mousePosition.x > 600)
        //    xOffset = -150;
        //else
        //    xOffset = 150;
        //float yOffset = 0;
        //if (mousePosition.y > 320)
        //    yOffset = -150;
        //else
        //    yOffset = 150;
        //ui.skillTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y+yOffset);
    }

    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName,out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
            _data.skillTree.Add(skillName, unlocked);
    }
}