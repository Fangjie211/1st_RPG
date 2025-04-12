using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private Animator anim;

    public string id;
    public bool activated;

    private void Start()
    {
        
        //if (string.IsNullOrEmpty(id))
        //{
        //    Debug.Log("ddd");
        //    GenerateId();
        //}
        anim = GetComponent<Animator>();
    }
    [ContextMenu("Generate ID")]
    private void GenerateId()
    {
        Debug.Log("d");
        id = System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            ActivateCheckPoint();
            Debug.Log("in");
        }
    }

    public void ActivateCheckPoint()
    {
        activated = true;
        anim.SetBool("active", true);
        SaveManager.instance.SaveGame();
    }
}
