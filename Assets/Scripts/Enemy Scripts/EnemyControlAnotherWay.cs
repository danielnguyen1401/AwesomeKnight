using UnityEngine;
using UnityEngine.AI;

public class EnemyControlAnotherWay : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    [SerializeField] private Transform[] WalkPoints;
    [SerializeField] private float walkDistance = 8f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float waitAttackTime = 1f;
    [SerializeField] private float rotateSpeed = 5f;

    private Vector3 nextDestination;
    private Transform playerTarget;
    private PlayerHealth playerHealth;
    private float currentAttackTime;
    private int walkIndex;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = playerTarget.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (enemyHealth.Health > 0)
        {
            if (playerTarget)
                MoveAndAttack();
        }
        else
        {
            anim.SetBool("Death", true);
            agent.enabled = false;
            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                Destroy(gameObject, 1.5f);
            }
        }
    }

    private void MoveAndAttack()
    {
        var distanceEnemyToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (distanceEnemyToPlayer > walkDistance)
        {
            if (agent.remainingDistance <= 0.5f) // enemy walk around Walk positions
            {
                agent.isStopped = false;
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                anim.SetInteger("Atk", 0);
                nextDestination = WalkPoints[walkIndex].position;
                agent.SetDestination(nextDestination);

                if (walkIndex == WalkPoints.Length - 1)
                    walkIndex = 0;
                else
                    walkIndex++;
            }
        }
        else
        {
            if (distanceEnemyToPlayer > attackDistance) // run to the player
            {
                agent.isStopped = false;
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                nextDestination = playerTarget.position;
                agent.SetDestination(nextDestination);
            }
            else // Attack the player
            {
                agent.isStopped = true;
                anim.SetBool("Run", false);

                Vector3 targetPos = new Vector3(playerTarget.position.x, transform.position.y,
                    playerTarget.position.z);

                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(targetPos - transform.position), rotateSpeed * Time.deltaTime);

                if (currentAttackTime >= waitAttackTime)
                {
                    anim.SetInteger("Atk", Random.Range(1, 3));
                    currentAttackTime = 0;
                }
                else
                {
                    anim.SetInteger("Atk", 0);
                    currentAttackTime += Time.deltaTime;
                }
            }
        }
    }
}