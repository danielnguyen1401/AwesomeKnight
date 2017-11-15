using UnityEngine;

public class SkillDamage : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private float _damageAmount = 10f;
    private EnemyHealth _enemyHealth;
    private bool _collided; // default is false

    void Start()
    {
    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _enemyLayerMask);

        foreach (Collider c in hits)
        {
            _enemyHealth = c.gameObject.GetComponent<EnemyHealth>();
            _collided = true;
        }
        if (_collided)
        {
            _enemyHealth.TakeDamage(_damageAmount);
            enabled = false; // Take damage to the enemy 1 time
        }
    }
}