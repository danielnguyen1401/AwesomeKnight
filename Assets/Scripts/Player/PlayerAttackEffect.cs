using UnityEngine;

public class PlayerAttackEffect : MonoBehaviour
{
    [SerializeField] Transform _groundImpactSpawn;
    [SerializeField] Transform _kickSpawn;
    [SerializeField] Transform _fireTornadoSpawn;
    [SerializeField] Transform _fireShieldSpawn;

    [SerializeField] GameObject _groundImpactFx;
    [SerializeField] GameObject _kickFx;
    [SerializeField] GameObject _fireTornadoFx;
    [SerializeField] GameObject _fireShieldFx;
    [SerializeField] GameObject _healFx;
    [SerializeField] GameObject _thunderAttackFx;

    void Start()
    {
    }

    void Update()
    {
    }

    void GroundImpact()
    {
        Instantiate(_groundImpactFx, _groundImpactSpawn.position, Quaternion.identity);
    }

    void Kick()
    {
        Instantiate(_kickFx, _kickSpawn.position, Quaternion.identity);
    }

    void FireTornado()
    {
        Instantiate(_fireTornadoFx, _fireTornadoSpawn.position, Quaternion.identity);
    }

    void FireShield()
    {
//        GameObject fireObj = Instantiate(_fireShieldFx, _fireShieldSpawn.position, Quaternion.identity) as GameObject;
//        fireObj.transform.SetParent(transform);
        Instantiate(_fireShieldFx, _fireShieldSpawn.position, Quaternion.identity);
    }

    void Heal()
    {
        Vector3 tempPos = transform.position;
        tempPos.y += 2f;
        GameObject healObj = Instantiate(_healFx, tempPos, Quaternion.identity) as GameObject;
        healObj.transform.SetParent(transform); // Fx will be inside of Black Knight Object

    }

    void ThunderAttack()
    {
    }
}