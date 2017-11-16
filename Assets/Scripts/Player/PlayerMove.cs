using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] CharacterController charController;
    [SerializeField] Animator anim;
    private CollisionFlags collisionFlags = CollisionFlags.None;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;
    bool canMove;
    bool finishedMovement = true;

    Vector3 targetPos = Vector3.zero;
    Vector3 playerMotion = Vector3.zero;


    float playerToPointDistance;
    private const float Gravity = 9.8f;
    private float height;

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
            height = 0f;
        else
            height -= Gravity * Time.deltaTime;
    }

    #region CheckIfFinishedMovement

    void CheckIfFinishedMovement()
    {
        if (!finishedMovement)
        {
            if (!anim.IsInTransition(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Stand") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                // normalized time of the animation is represented from 0 to 1, ... 0 is the beginning of the animation
                finishedMovement = true;
            }
        }
        else
        {
            MoveThePlayer();
            playerMotion.y = height * Time.deltaTime;
            collisionFlags = charController.Move(playerMotion);
        }
    }

    #endregion

    #region MoveThePlayer

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
                    playerToPointDistance = Vector3.Distance(transform.position, hit.point);
                    if (playerToPointDistance >= 1.0f)
                    {
                        canMove = true;
                        targetPos = hit.point;
                    }
                }
            }
        } // if mouse button down

        if (canMove)
        {
            anim.SetFloat("Walk", 1.0f);
            Vector3 targetTemp = new Vector3(targetPos.x, transform.position.y, targetPos.z);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(targetTemp - transform.position), rotationSpeed * Time.deltaTime);
            playerMotion = transform.forward * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPos) <= 0.5f)
                canMove = false;
        }
        else
        {
            playerMotion.Set(0.0f, 0.0f, 0.0f);
            anim.SetFloat("Walk", 0.0f);
        }

        charController.Move(playerMotion);
    }

    #endregion

    public bool FinishedMovement
    {
        get { return finishedMovement; }
        set { finishedMovement = value; }
    }

    public Vector3 TargetPosition
    {
        get { return targetPos; }
        set { targetPos = value; }
    }

    public float RotationSpeed
    {
        get { return rotationSpeed; }
    }
}