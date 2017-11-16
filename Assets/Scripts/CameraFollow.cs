using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float follow_Height = 8.0f;
    [SerializeField] float follow_Distance = 6f;

    [SerializeField] Transform player;

    private float targetHeight;
    private float currentHeight;
    private float currentRotation;

    void Awake()
    {
//        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player)
        {
            targetHeight = player.position.y + follow_Height;
            currentRotation = transform.eulerAngles.y;
            currentHeight = Mathf.Lerp(transform.position.y, targetHeight, 0.9f * Time.deltaTime);

            Quaternion euler = Quaternion.Euler(0, currentRotation, 0);

            Vector3 targetPosition = player.position - (euler * Vector3.forward) * follow_Distance;
            targetPosition.y = currentHeight;
            transform.position = targetPosition;
            transform.LookAt(player);
        }
    }
}