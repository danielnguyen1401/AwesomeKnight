using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float follow_Height = 8.0f;
    [SerializeField] float follow_Distance = 6f;

    [SerializeField] Transform player;

    float _targetHeight;
    float _currentHeight;
    float _currentRotation;

    void Awake()
    {
//        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        _targetHeight = player.position.y + follow_Height;
        _currentRotation = transform.eulerAngles.y;
        _currentHeight = Mathf.Lerp(transform.position.y, _targetHeight, 0.9f * Time.deltaTime);

        Quaternion euler = Quaternion.Euler(0, _currentRotation, 0);

        Vector3 targetPosition = player.position - (euler * Vector3.forward) * follow_Distance;
        targetPosition.y = _currentHeight;
        transform.position = targetPosition;
        transform.LookAt(player);
    }
}