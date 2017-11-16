using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isShielded;
    private Animator anim;

    void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        if (!isShielded)
        {
            currentHealth -= amount;
            if (currentHealth <= 0) // Todo: player die
            {
                currentHealth = 0;
                anim.SetBool("Death", true);
                if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
                {
                    Destroy(gameObject);
                }
            }
            Debug.Log("Player took damge, health: " + currentHealth);
        }
    }

    public void HealthPlayer(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public bool Shielded
    {
        get { return isShielded; }
        set { isShielded = value; }
    }

    public float Health
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
}