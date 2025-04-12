using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{


    [Header("EndScreen")]
    [SerializeField] private GameObject endtext;
    [SerializeField] public UI_FadeScreen fadeScreen;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skilltreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillTooltip skillTooltip;
    // Start is called before the first frame update

    private void Awake()
    {
        SwitchTo(skilltreeUI);
    }
    void Start()
    {
        
        SwitchTo(inGameUI);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(skilltreeUI);
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(optionsUI);
        }
    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>()!=null;
            if (!fadeScreen)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                
            }
        }
        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu!=null&& _menu.activeSelf)
        {
            _menu.SetActive(false);
            SwitchTo(inGameUI);
            return;
        }
        else
        {
            SwitchTo(_menu);
        }
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut(); 
        StartCoroutine(EndScreenCorutine());
    }

    [SerializeField] private GameObject restartButton;
    IEnumerator EndScreenCorutine()
    {
        yield return new WaitForSeconds(1f);
        endtext.SetActive(true);
        yield return new WaitForSeconds(1f);
        restartButton.SetActive(true);
    }


    public void RestartGameButton()=>GameManager.instance.RestartScene();
}