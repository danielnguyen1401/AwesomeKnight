using UnityEngine;

public class BossSpecialAttack : MonoBehaviour
{
    [SerializeField] private GameObject spellFx;
    [SerializeField] private GameObject fireTornadoFx;

    private Transform playerTarget;

    void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
    }

    void BossSpell()
    {
        Vector3 temp = playerTarget.position;
        temp.y += 1.5f;
        Instantiate(spellFx, temp, Quaternion.identity);
    }

    void BossFireTornado()
    {
        Instantiate(fireTornadoFx, playerTarget.position, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
    }
}