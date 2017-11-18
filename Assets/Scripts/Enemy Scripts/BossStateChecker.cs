using UnityEngine;

public enum BossState
{
    NONE,
    IDLE,
    PAUSE,
    ATTACK,
    DEATH
}

public class BossStateChecker : MonoBehaviour
{
    [SerializeField] float attackDistance = 1.5f;
    [SerializeField] float followDistance = 15f;

    private Transform playerTarget;
    private BossState bossState = BossState.NONE;
    private float distanceToTarget;
    private EnemyHealth bossHealth;


    void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        bossHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        SetState();
    }

    void SetState()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (bossState != BossState.DEATH)
        {
            if (distanceToPlayer > attackDistance && distanceToPlayer <= followDistance)
                bossState = BossState.PAUSE;

            else if (distanceToPlayer <= attackDistance)
                bossState = BossState.ATTACK;

            else if (distanceToPlayer > followDistance)
                bossState = BossState.IDLE;

            else
                bossState = BossState.DEATH;

            if (bossHealth.Health <= 0)
                bossState = BossState.DEATH;
        }
    }

    public BossState BossState
    {
        get { return bossState; }
        set { bossState = value; }
    }

    public float AttackDistance
    {
        get { return attackDistance; }
        set { attackDistance = value; }
    }
}