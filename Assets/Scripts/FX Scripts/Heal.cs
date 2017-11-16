using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private float _healAmount = 10f;

    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().HealthPlayer(_healAmount) ;
    }

    void Update()
    {
    }
}