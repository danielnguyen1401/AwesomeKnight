using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            health = 0;
    }

    public float Health
    {
        get { return health; }
        set { health = value; }
    }
}