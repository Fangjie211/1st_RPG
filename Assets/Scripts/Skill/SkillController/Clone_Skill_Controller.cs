using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{



    private Transform closestEnemy;
    private SpriteRenderer sr;
    private Animator anim;
    float colorLosingSpeed;
    private float cloneTimer;
    private bool canDuplicate;
    private int facingDir = 1;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;


    private float chanceToDuplicate;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));
        }
        if (sr.color.a <= 0)
            Destroy(gameObject);
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, float _colorLosingSpeed, bool _canAttack, Vector3 _offset, Transform _closestEnemy,bool _canDuplicate,float _chanceToDuplicate)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        }
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        colorLosingSpeed = _colorLosingSpeed;
        closestEnemy = _closestEnemy;
        FaceClosestTarget();
        canDuplicate = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null){
                hit.GetComponent<Enemy>().Damage();

                if (canDuplicate)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f*facingDir, 0));
                    }
                }
            }
        }
    }
    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
