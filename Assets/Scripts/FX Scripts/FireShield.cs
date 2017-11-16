using UnityEngine;

public class FireShield : MonoBehaviour
{
    private PlayerHealth _playerHealth;

    void Awake()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }


    void OnEnable()
    {
        _playerHealth.Shielded = true;
    }

    void OnDisable()
    {
        _playerHealth.Shielded = false;
    }


    void Update()
    {
    }
}