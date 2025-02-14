using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed=12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate=true;
    private bool isReturning;
    [Header("Bounce info")]
    [SerializeField]private  float bounceSpeed;

    private bool isBouncing;
    private int amountOfBounce;

    [Header("Pierce info")]
    private int pierceAmount;

    [Header("Spin info")]
    private float spinDuration;
    private float maxTravelDistance;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;


    private float spinDirection;

    private List<Transform> enemyTarget;//if it is public ,created defaultly by unity,but private should set by myself
    private int targetIndex;
    private void Awake()
    {
        anim=GetComponentInChildren<Animator>();
        rb=GetComponent<Rigidbody2D>();
        cd=GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir,float _gravityScale,Player _player)
    {
        player= _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        if(pierceAmount<=0) 
            anim.SetBool("Rotation", true);

        spinDirection=Mathf.Clamp(rb.velocity.x,-1,1);
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount= _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning,float _maxTravelDistance,float _spinDuration,float _hitCooldown)
    {
        isSpinning= _isSpinning;
        maxTravelDistance= _maxTravelDistance;
        spinDuration= _spinDuration;
        hitCooldown= _hitCooldown;
    }
    public void SetupBounce(bool _isBouncing,int _amountOfBounces)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounces;
        enemyTarget = new List<Transform>();
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }
    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 2)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpining();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                transform.position=Vector2.MoveTowards(transform.position,new Vector2(transform.position.x+spinDirection,transform.position.y),1.5f*Time.deltaTime);
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            hit.GetComponent<Enemy>().Damage();
                        }



                    }

                }
            }
        }
    }

    private void StopWhenSpining()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                enemyTarget[targetIndex].GetComponent<Enemy>().Damage();
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce < 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }



                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        collision.GetComponent<Enemy>()?.Damage();
        SetupTargetForBounce(collision);
        Stuckinto(collision);
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private void Stuckinto(Collider2D collision)
    {

        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning){
            StopWhenSpining();
            return;
        }
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        
        rb.constraints = RigidbodyConstraints2D.FreezeAll;


        if (isBouncing&&enemyTarget.Count>1)
            return;
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
