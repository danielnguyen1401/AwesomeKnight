using UnityEngine;
using UnityEngine.AI;

public class BossControl : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 5f;
    [SerializeField] float waitAttackTime = 1f;
    private Transform playerTarget;
    BossStateChecker bossStateChecker;
    private NavMeshAgent agent;
    private Animator anim;

    private bool finishedAttacking = true;
    private float currentAttackTime;

    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        bossStateChecker = GetComponent<BossStateChecker>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = bossStateChecker.AttackDistance;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (finishedAttacking)
        {
            GetStateControl();
        }
        else
        {
            anim.SetInteger("Atk", 0);

            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                finishedAttacking = true;
            }
        }
    }

    void GetStateControl()
    {
        if (bossStateChecker.BossState == BossState.DEATH)
        {
            agent.isStopped = true;
            anim.SetBool("Death", true);
            Destroy(gameObject, 2f);
        }
        else
        {
            if (bossStateChecker.BossState == BossState.PAUSE)
            {
                agent.isStopped = false;
                anim.SetBool("Run", true);
                agent.SetDestination(playerTarget.position);
            }
            else if (bossStateChecker.BossState == BossState.ATTACK)
            {
                anim.SetBool("Run", false);
                if (playerTarget)
                {
                    Vector3 targetPos = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), Time.deltaTime * rotateSpeed);
                }
                if (currentAttackTime >= waitAttackTime)
                {
                    int atkRange = Random.Range(3, 5);
                    anim.SetInteger("Atk", atkRange);
                    currentAttackTime = 0;
                    finishedAttacking = false;
                }
                else
                {
                    anim.SetInteger("Atk", 0);
                    currentAttackTime += Time.deltaTime;
                }
            }
            else
            {
                anim.SetBool("Run", false);
                agent.isStopped = true;
            }
        }
    }
}