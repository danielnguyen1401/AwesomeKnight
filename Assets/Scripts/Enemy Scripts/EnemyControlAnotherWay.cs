using UnityEngine;
using UnityEngine.AI;

public class EnemyControlAnotherWay : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _anim;

    [SerializeField] private Transform[] WalkPoints;
    [SerializeField] private float _walkDistance = 8f;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _waitAttackTime = 1f;
    [SerializeField] private float _rotateSpeed = 5f;

    private Vector3 _nextDestination;
    private Transform _playerTarget;
    private float _currentAttackTime;
    private int _walkIndex;
    private EnemyHealth _enemyHealth;

    private void Awake()
    {
        _playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (_enemyHealth.Health > 0)
        {
            MoveAndAttack();
        }
        else
        {
            _anim.SetBool("Death", true);
            _agent.enabled = false;
            if (!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
                _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                Destroy(gameObject, 2f);
            }
        }
    }

    private void MoveAndAttack()
    {
        var distanceEnemyToPlayer = Vector3.Distance(transform.position, _playerTarget.position);

        if (distanceEnemyToPlayer > _walkDistance)
        {
            if (_agent.remainingDistance <= 0.5f) // enemy walk around Walk positions
            {
                _agent.isStopped = false;
                _anim.SetBool("Walk", true);
                _anim.SetBool("Run", false);
                _anim.SetInteger("Atk", 0);
                _nextDestination = WalkPoints[_walkIndex].position;
                _agent.SetDestination(_nextDestination);

                if (_walkIndex == WalkPoints.Length - 1)
                    _walkIndex = 0;
                else
                    _walkIndex++;
            }
        }
        else
        {
            if (distanceEnemyToPlayer > _attackDistance) // run to the player
            {
                _agent.isStopped = false;
                _anim.SetBool("Run", true);
                _anim.SetBool("Walk", false);
                _nextDestination = _playerTarget.position;
                _agent.SetDestination(_nextDestination);
            }
            else // Attack the player
            {
                _agent.isStopped = true;
                _anim.SetBool("Run", false);

                Vector3 targetPos = new Vector3(_playerTarget.position.x, transform.position.y,
                    _playerTarget.position.z);

                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(targetPos - transform.position), _rotateSpeed * Time.deltaTime);

                if (_currentAttackTime >= _waitAttackTime)
                {
                    _anim.SetInteger("Atk", Random.Range(1, 3));
                    _currentAttackTime = 0;
                }
                else
                {
                    _anim.SetInteger("Atk", 0);
                    _currentAttackTime += Time.deltaTime;
                }
            }
        }
    }
}