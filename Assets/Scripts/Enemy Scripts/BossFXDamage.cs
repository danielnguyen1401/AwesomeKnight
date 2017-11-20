using UnityEngine;

public class BossFXDamage : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float radius = 0.3f;
    [SerializeField] private float damageCount = 10f;

    private PlayerHealth playerHealth;
    private bool collided;
    void Awake()
    {

    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, playerLayerMask);

        foreach (var c in hits)
        {
            playerHealth = c.GetComponent<PlayerHealth>();
            collided = true;
        }
        if (collided)
        {
            playerHealth.TakeDamage(damageCount);
            enabled = false;
        }
    }
}