using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float timer = 2f;

    void Start()
    {
        Destroy(gameObject, timer);
    }

    void Update()
    {
    }
}