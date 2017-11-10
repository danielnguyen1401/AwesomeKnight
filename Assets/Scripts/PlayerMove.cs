using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] CharacterController charController;
    CollisionFlags collisionFlags = CollisionFlags.None;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 15f;
    bool _canMove;
    bool _finishedMovement = false;

    Vector3 _targetPos = Vector3.zero;
    Vector3 _playerMotion = Vector3.zero;

    float _playerToPointDistance;
    private const float Gravity = 9.8f;
    private float _height;

    void Awake()
    {
        // get animator by drag into [SerializeField]
        // get charactercontroller
    }

    void Start()
    {
    }

    void Update()
    {
        CalculateHeight();
        CheckIfFinishedMovement();
    }

    bool IsGrounded()
    {
        return collisionFlags == CollisionFlags.CollidedBelow ? true : false;
    }

    void CalculateHeight()
    {
        if (IsGrounded())
            _height = 0f;
        else
            _height -= Gravity * Time.deltaTime;
    }

    void CheckIfFinishedMovement()
    {
        if (!_finishedMovement)
        {
            if (!anim.IsInTransition(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Stand") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                // normalized time of animation is represented from 0 to 1
                // 0 is the begining of animation
                _finishedMovement = true;
            }
            else
            {
                MoveThePlayer();
                _playerMotion.y = _height * Time.deltaTime;
                collisionFlags = charController.Move(_playerMotion);
            }
        }
    }

    void MoveThePlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider is TerrainCollider)
                {
                    // screen point and world point is different
                    _playerToPointDistance = Vector3.Distance(transform.position, hit.point);
                    if (_playerToPointDistance > 1.0f)
                    {
                        _canMove = true;
                        _targetPos = hit.point;
                    }
                }
            }
            
        } // if mouse button down

        if (_canMove)
        {
            anim.SetFloat("Walk", 1.0f);
            Vector3 targetTemp = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetTemp - transform.position), rotationSpeed * Time.deltaTime);
            _playerMotion = transform.forward * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, _targetPos) <= 0.1f)
                _canMove = false;
        }
        else
        {
            _playerMotion.Set(0.0f, 0.0f, 0.0f);
            anim.SetFloat("Walk", 0.0f);
        }
        charController.Move(_playerMotion);
    }
}