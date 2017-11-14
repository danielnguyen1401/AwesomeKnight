using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _anim;

    [SerializeField] private float _alertAttackDistance = 8f;
    [SerializeField] private float _attackDistance = 1.5f;
    [SerializeField] private float _enemyToPlayerDistance = 15f;
    [SerializeField] private float _followDistance = 15f;
    [SerializeField] private float _waitAttackTime = 0.5f;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 5f;


    private float _currentAttackTime;
    private EnemyState _enemyCurrentState = EnemyState.Idle;
    private EnemyState _enemyLastState = EnemyState.Idle;

//    private bool _finishedAnim = true;
    private bool _finishedMovement = true;

    private Vector3 _initialPosition;
    private Transform _playerTarget;

//    private Vector3 _whereToMove = Vector3.zero;
    private Vector3 _whereToNavigate = Vector3.zero;

    private EnemyHealth _enemyHealth;

    void Awake()
    {
        _playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _enemyHealth = GetComponent<EnemyHealth>();

        _initialPosition = transform.position;
        _whereToNavigate = transform.position;
    }

    void Update()
    {
        if (_enemyHealth.Health <= 0)
            _enemyCurrentState = EnemyState.Death;

        if (_enemyCurrentState != EnemyState.Death) // if enemy is alive
        {
            _enemyCurrentState = SetEnemyState(_enemyCurrentState, _enemyLastState, _enemyToPlayerDistance);

            if (_finishedMovement)
            {
                GetStateControl(_enemyCurrentState);
            }
            else
            {
                if (!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) // is in IDLE animation
                {
                    _finishedMovement = true;
                }
                else if (!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsTag("Atk1") || _anim.GetCurrentAnimatorStateInfo(0).IsTag("Atk2"))
                {
                    _anim.SetInteger("Atk", 0);
                }
            }
        }
        else // if the enemy is death
        {
            _anim.SetBool("Death", true);
            _agent.enabled = false;

            if (!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
                _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                Destroy(gameObject, 2f);
            }
        } // end if 
    }

    EnemyState SetEnemyState(EnemyState curState, EnemyState lastState, float enemyToPlayerDis)
    {
        float initialDistance = Vector3.Distance(_initialPosition, transform.position);
        enemyToPlayerDis = Vector3.Distance(transform.position, _playerTarget.position);

        if (initialDistance > _followDistance)
        {
            lastState = curState;
            curState = EnemyState.GoBack;
        }
        else if (enemyToPlayerDis <= _attackDistance)
        {
            lastState = curState;
            curState = EnemyState.Attack;
        }
        else if (enemyToPlayerDis >= _alertAttackDistance && lastState == EnemyState.Pause ||
                 lastState == EnemyState.Attack)
        {
            lastState = curState;
            curState = EnemyState.Pause;
        }
        else if (enemyToPlayerDis <= _alertAttackDistance && enemyToPlayerDis > _attackDistance)
        {
            if (curState != EnemyState.GoBack || lastState == EnemyState.Walk)
            {
                lastState = curState;
                curState = EnemyState.Pause;
            }
        }
        else if (enemyToPlayerDis > _alertAttackDistance && lastState != EnemyState.GoBack ||
                 lastState != EnemyState.Pause)
        {
            lastState = curState;
            curState = EnemyState.Walk;
        }
        return curState;
    }

    void GetStateControl(EnemyState curState)
    {
        if (curState == EnemyState.Pause || curState == EnemyState.Run)
        {
            if (curState != EnemyState.Attack)
            {
                Vector3 targetPos = new Vector3(_playerTarget.position.x, transform.position.y, _playerTarget.position.z);

                if (Vector3.Distance(targetPos, transform.position) > 2.1f)
                {
                    _anim.SetBool("Walk", false);
                    _anim.SetBool("Run", true);

                    _agent.speed = _moveSpeed;
                    _agent.SetDestination(targetPos);
                }
            }
        }
        else if (curState == EnemyState.Attack)
        {
            _anim.SetBool("Run", false);
//            _whereToMove.Set(0, 0, 0);
            Vector3 targetPos = new Vector3(_playerTarget.position.x, transform.position.y, _playerTarget.position.z);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), _rotateSpeed * Time.deltaTime);

            _agent.speed = _walkSpeed;
            _agent.SetDestination(transform.position);

            if (_currentAttackTime >= _waitAttackTime)
            {
                int atkRange = Random.Range(1, 3);
                _anim.SetInteger("Atk", atkRange);
//                _finishedAnim = false;
                _currentAttackTime = 0;
            }
            else
            {
                _anim.SetInteger("Atk", 0);
                _currentAttackTime += Time.deltaTime;
            }
        }
        else if (curState == EnemyState.GoBack)
        {
            _anim.SetBool("Run", true);

            Vector3 targetPos = new Vector3(_initialPosition.x, transform.position.y, _initialPosition.z);
            _agent.speed = _moveSpeed;
            _agent.SetDestination(targetPos);

            if (Vector3.Distance(targetPos, _initialPosition) <= 3.5f)
            {
                _enemyLastState = curState;
                curState = EnemyState.Walk;
            }
        }
        else if (curState == EnemyState.Walk)
        {
            _anim.SetBool("Run", false);
            _anim.SetBool("Walk", true);

            if (Vector3.Distance(transform.position, _whereToNavigate) <= 2.0f)
            {
                _whereToNavigate.x = Random.Range(_initialPosition.x - 5f, _initialPosition.x + 5f);
                _whereToNavigate.z = Random.Range(_initialPosition.z - 5f, _initialPosition.z + 5f);
            }
            else
            {
                _agent.speed = _walkSpeed;
                _agent.SetDestination(_whereToNavigate);
            }
        }
        else
        {
            _agent.isStopped = true;
            _anim.SetBool("Run", false);
            _anim.SetBool("Walk", false);
        }
    }
}


public enum EnemyState
{
    Idle,
    Walk,
    Run,
    Pause,
    GoBack,
    Attack,
    Death
}