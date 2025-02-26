using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{

    private Player player;

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

    public void SetupClone(Transform _newTransform, float _cloneDuration, float _colorLosingSpeed, bool _canAttack, Vector3 _offset, Transform _closestEnemy,bool _canDuplicate,float _chanceToDuplicate,int _OriginFacingDir,Player _player)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        }
        player= _player;
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        colorLosingSpeed = _colorLosingSpeed;
        closestEnemy = _closestEnemy;
        FaceClosestTarget(_OriginFacingDir);
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
                player.stats.DoDamage(hit.GetComponent<CharacterStats>());

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
    private void FaceClosestTarget(int originFacing)
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
        else
        {
            Debug.Log("ddd");
            if (originFacing == 1) { return; }
            else { transform.Rotate(0, 180, 0); }
        }
    }
}
