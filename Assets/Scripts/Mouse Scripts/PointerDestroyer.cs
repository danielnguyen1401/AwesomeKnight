using UnityEngine;

public class PointerDestroyer : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private float _distanceToDestroy; // 1.1f

    void Start()
    {
//        Destroy(gameObject, _distanceToDestroy);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    void Update()
    {
        if (Vector3.Distance(transform.position, _player.position) <= _distanceToDestroy)
        {
            Destroy(gameObject);
        }
    }
}