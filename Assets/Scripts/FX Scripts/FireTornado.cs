using UnityEngine;

public class FireTornado : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _radius = 1f;
    [SerializeField] private float _damageAmount = 20f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private GameObject _fireExplosion;
    private EnemyHealth _enemyHealth;

    private bool _collided;
    private Transform _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.rotation = Quaternion.LookRotation(_player.forward);
    }

    void Update()
    {
        MoveForward();
        CheckForDamage();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * (_moveSpeed * Time.deltaTime));
    }

    private void CheckForDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _enemyLayerMask);

        foreach (Collider c in hits)
        {
            _enemyHealth = c.gameObject.GetComponent<EnemyHealth>();
            _collided = true;
        }
        if (_collided)
        {
            _enemyHealth.TakeDamage(_damageAmount);
            Vector3 temp = transform.position;
            temp.y += 1.0f;
            Instantiate(_fireExplosion, temp, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}