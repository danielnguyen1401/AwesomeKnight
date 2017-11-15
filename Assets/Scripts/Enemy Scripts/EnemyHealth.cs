using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float _health = 100f;

    void Start()
    {
    }

    void Update()
    {
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            _health = 0;
        }
        Debug.Log("Health: " + _health);
    }

    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }
}