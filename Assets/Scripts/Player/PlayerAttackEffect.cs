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
        GameObject fireObj = Instantiate(_fireShieldFx, _fireShieldSpawn.position, Quaternion.identity) as GameObject;
        fireObj.transform.SetParent(transform);
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
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < 8; i++) //
        {
            if (i == 0)
                pos = new Vector3(transform.position.x - 4f, transform.position.y + 2f, transform.position.z);
            else if (i == 1)
                pos = new Vector3(transform.position.x + 4f, transform.position.y + 2f, transform.position.z);
            else if (i == 2)
                pos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z - 4f);
            else if (i == 3)
                pos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z + 4f);
            else if (i == 4)
                pos = new Vector3(transform.position.x + 2.5f, transform.position.y + 2f, transform.position.z + 2.5f);
            else if (i == 5)
                pos = new Vector3(transform.position.x - 2.5f, transform.position.y + 2f, transform.position.z + 2.5f);
            else if (i == 6)
                pos = new Vector3(transform.position.x - 2.5f, transform.position.y + 2f, transform.position.z - 2.5f);
            else if (i == 7)
                pos = new Vector3(transform.position.x + 2.5f, transform.position.y + 2f, transform.position.z + 2.5f);

            Instantiate(_thunderAttackFx, pos, Quaternion.identity);
        }
    }
}