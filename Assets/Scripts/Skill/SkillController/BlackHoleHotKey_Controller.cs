using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotKey_Controller : MonoBehaviour
{

    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform enemiesTransform;

    private BlackHole_Skill_Controller blackHole;
    public void SetupHotKey(KeyCode _myHotKey,Transform _myEnemy,BlackHole_Skill_Controller _myBlackHole)
    {
        sr= GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myText.text=_myHotKey.ToString();
        myHotKey = _myHotKey;
        enemiesTransform = _myEnemy;
        blackHole = _myBlackHole;
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey)){
            blackHole.AddEnemyToList(enemiesTransform);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }


    }

}
