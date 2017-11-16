using UnityEngine;

public class SkillDamage : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private float damageAmount = 10f;
    private EnemyHealth enemyHealth;
    private bool collided; // default is false

    void Start()
    {
    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayerMask);

        foreach (Collider c in hits)
        {
            enemyHealth = c.gameObject.GetComponent<EnemyHealth>();
            collided = true;
        }
        if (collided)
        {
            enemyHealth.TakeDamage(damageAmount);
            enabled = false; // Take damage to the enemy 1 time
        }
    }
}