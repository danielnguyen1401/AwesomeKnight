using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private Image healthBG; // set it to: Health Foreground Image
    private float currentHealth;

    void Awake()
    {
//        if (tag == "Boss")
//            healthBG = GameObject.Find("Health Enemy BG Boss").GetComponent<Image>();
//        else
//            healthBG = GameObject.Find("Health Enemy BG").GetComponent<Image>();

        currentHealth = health;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBG.transform.GetChild(0).GetComponent<Image>().fillAmount = currentHealth / health;
        if (currentHealth <= 0)
            currentHealth = 0;
    }

    public float Health
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    void Update()
    {
        healthBG.transform.LookAt(Camera.main.transform);
    }
}