using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    public void ShowToolTip(string _text,string _skillName)
    {
        skillName.text = _skillName;
        skillText.text = _text;
        gameObject.SetActive(true);
    }
    public void HideToolTip()=>gameObject.SetActive(false);
}
