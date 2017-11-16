using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float damageDistance = 1.5f;

    private Animator anim;
    private Transform playerTarget;
    private bool finishedAttack = true;
    private PlayerHealth _playerHealth;

    void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        _playerHealth = playerTarget.GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (finishedAttack)
        {
            DealDamage(CheckIfAttacking());
        }
        else
        {
            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                finishedAttack = true;
        }
    }

    bool CheckIfAttacking()
    {
        bool isAttacking = false;

        if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Atk1") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Atk2"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
            {
                isAttacking = true;
                finishedAttack = false;
            }
        }
        return isAttacking;
    }


    void DealDamage(bool isAttacking)
    {
        if (playerTarget)
        {
            if (isAttacking)
            {
                if (Vector3.Distance(transform.position, playerTarget.position) <= damageDistance)
                {
                    _playerHealth.TakeDamage(damageAmount);
                }
            }
        }
    }
}