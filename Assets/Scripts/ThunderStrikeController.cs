using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private Animator anim;
    private bool triggered;
    private int damage;

    private void Start()
    {
        anim=GetComponentInChildren<Animator>();

    }
    public void Setup(int _damage,CharacterStats _targetStats)
    {
        damage= _damage;
        targetStats= _targetStats;
    }
    private void Update()
    {
        if (!targetStats)
        {
            return;
        }
        if (triggered)
            return;
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right= transform.position-targetStats.transform.position;
        if(Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {

            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            anim.transform.localPosition=new Vector3(0,.5f);

            triggered = true;
            anim.SetTrigger("Hit");
            Invoke("DamageAndSelfDestory", .3f);
        }
    }

    private void DamageAndSelfDestory()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}
