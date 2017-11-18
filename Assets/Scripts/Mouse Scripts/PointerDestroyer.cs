using UnityEngine;

public class PointerDestroyer : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private float _distanceToDestroy; // 1.1f

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        if (_player)
        {
            if (Vector3.Distance(transform.position, _player.position) <= _distanceToDestroy)
                Destroy(gameObject);
        }
    }
}