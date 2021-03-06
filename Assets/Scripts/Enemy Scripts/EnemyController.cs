﻿using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    [SerializeField] private float alertAttackDistance = 8f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float enemyToPlayerDistance = 15f;
    [SerializeField] private float followDistance = 15f;
    [SerializeField] private float waitAttackTime = 0.5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float rotateSpeed = 5f;

    private float currentAttackTime;
    private EnemyState enemyCurrentState = EnemyState.IDLE;
    private EnemyState enemyLastState = EnemyState.IDLE;

    private bool finishedMovement = true;

    private Vector3 initialPosition;
    private Transform playerTarget;

//    private Vector3 _whereToMove = Vector3.zero;
    private Vector3 whereToNavigate = Vector3.zero;

    private EnemyHealth enemyHealth;

    void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();

        initialPosition = transform.position;
        whereToNavigate = transform.position;
    }

    void Update()
    {
        if (enemyHealth.Health <= 0)
            enemyCurrentState = EnemyState.DEATH;

        if (enemyCurrentState != EnemyState.DEATH) // if enemy is alive and player is alive
        {
            if (playerTarget)
            {
                enemyCurrentState = SetEnemyState(enemyCurrentState, enemyLastState, enemyToPlayerDistance);

                if (finishedMovement)
                    GetStateControl(enemyCurrentState);
                else
                {
                    if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        // is in IDLE animation
                        finishedMovement = true;
                    }
                    else if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsTag("Atk1") ||
                             anim.GetCurrentAnimatorStateInfo(0).IsTag("Atk2"))
                    {
                        anim.SetInteger("Atk", 0);
                    }
                }
            }
        }
        else // if the enemy is death
        {
            anim.SetBool("Death", true);
            agent.enabled = false;

            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                Destroy(gameObject, 2f);
            }
        } // end if 
    }

    EnemyState SetEnemyState(EnemyState curState, EnemyState lastState, float enemyToPlayerDis)
    {
        float initialDistance = Vector3.Distance(initialPosition, transform.position);
        enemyToPlayerDis = Vector3.Distance(transform.position, playerTarget.position);

        if (initialDistance > followDistance)
        {
            lastState = curState;
            curState = EnemyState.GOBACK;
        }
        else if (enemyToPlayerDis <= attackDistance)
        {
            lastState = curState;
            curState = EnemyState.ATTACK;
        }
        else if (enemyToPlayerDis >= alertAttackDistance && lastState == EnemyState.PAUSE ||
                 lastState == EnemyState.ATTACK)
        {
            lastState = curState;
            curState = EnemyState.PAUSE;
        }
        else if (enemyToPlayerDis <= alertAttackDistance && enemyToPlayerDis > attackDistance)
        {
            if (curState != EnemyState.GOBACK || lastState == EnemyState.WALK)
            {
                lastState = curState;
                curState = EnemyState.PAUSE;
            }
        }
        else if (enemyToPlayerDis > alertAttackDistance && lastState != EnemyState.GOBACK ||
                 lastState != EnemyState.PAUSE)
        {
            lastState = curState;
            curState = EnemyState.WALK;
        }
        return curState;
    }

    void GetStateControl(EnemyState curState)
    {
        if (curState == EnemyState.PAUSE || curState == EnemyState.RUN)
        {
            if (curState != EnemyState.ATTACK)
            {
                Vector3 targetPos = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);

                if (Vector3.Distance(targetPos, transform.position) > 2.1f)
                {
                    anim.SetBool("Walk", false);
                    anim.SetBool("Run", true);

                    agent.speed = moveSpeed;
                    agent.SetDestination(targetPos);
                }
            }
        }
        else if (curState == EnemyState.ATTACK)
        {
            anim.SetBool("Run", false);
//            _whereToMove.Set(0, 0, 0);
            Vector3 targetPos = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(targetPos - transform.position), rotateSpeed * Time.deltaTime);

            agent.speed = walkSpeed;
            agent.SetDestination(transform.position);

            if (currentAttackTime >= waitAttackTime)
            {
                int atkRange = Random.Range(1, 3);
                anim.SetInteger("Atk", atkRange);
                currentAttackTime = 0;
            }
            else
            {
                anim.SetInteger("Atk", 0);
                currentAttackTime += Time.deltaTime;
            }
        }
        else if (curState == EnemyState.GOBACK)
        {
            anim.SetBool("Run", true);

            Vector3 targetPos = new Vector3(initialPosition.x, transform.position.y, initialPosition.z);
            agent.speed = moveSpeed;
            agent.SetDestination(targetPos);

            if (Vector3.Distance(targetPos, initialPosition) <= 3.5f)
            {
                enemyLastState = curState;
                curState = EnemyState.WALK;
            }
        }
        else if (curState == EnemyState.WALK)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Walk", true);

            if (Vector3.Distance(transform.position, whereToNavigate) <= 2.0f)
            {
                whereToNavigate.x = Random.Range(initialPosition.x - 5f, initialPosition.x + 5f);
                whereToNavigate.z = Random.Range(initialPosition.z - 5f, initialPosition.z + 5f);
            }
            else
            {
                agent.speed = walkSpeed;
                agent.SetDestination(whereToNavigate);
            }
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
        }
    }
}


public enum EnemyState
{
    IDLE,
    WALK,
    RUN,
    PAUSE,
    GOBACK,
    ATTACK,
    DEATH
}