using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Image _fillWaitImgImage1;
    [SerializeField] private Image _fillWaitImgImage2;
    [SerializeField] private Image _fillWaitImgImage3;
    [SerializeField] private Image _fillWaitImgImage4;
    [SerializeField] private Image _fillWaitImgImage5;
    [SerializeField] private Image _fillWaitImgImage6;

    private int[] fadeImages = new int[] {0, 0, 0, 0, 0, 0};
    private Animator anim;
    private PlayerMove playerMove;
    private bool canAttack = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
            canAttack = true;
        else
            canAttack = false;

        CheckToFade();
        CheckInput();
    }

    #region CheckInput

    void CheckInput()
    {
        if (anim.GetInteger("Atk") == 0)
        {
            playerMove.FinishedMovement = false;

            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
            {
                playerMove.FinishedMovement = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Ground Impact
        {
            if (playerMove.FinishedMovement && fadeImages[0] != 1 && canAttack)
            {
                fadeImages[0] = 1;
                anim.SetInteger("Atk", 1);
                playerMove.TargetPosition = transform.position;
                RemoveCursor();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Kick
        {
            if (playerMove.FinishedMovement && fadeImages[1] != 1 && canAttack)
            {
                fadeImages[1] = 1;
                anim.SetInteger("Atk", 2);
                playerMove.TargetPosition = transform.position;
                RemoveCursor();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Fire Tornado
        {
            if (playerMove.FinishedMovement && fadeImages[2] != 1 && canAttack)
            {
                fadeImages[2] = 1;
                anim.SetInteger("Atk", 3);
                playerMove.TargetPosition = transform.position;
                RemoveCursor();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // Fire Shield
        {
            if (playerMove.FinishedMovement && fadeImages[3] != 1 && canAttack)
            {
                fadeImages[3] = 1;
                anim.SetInteger("Atk", 4);
                playerMove.TargetPosition = transform.position;
                RemoveCursor();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) // Health
        {
            if (playerMove.FinishedMovement && fadeImages[4] != 1 && canAttack)
            {
                fadeImages[4] = 1;
                anim.SetInteger("Atk", 5);
                playerMove.TargetPosition = transform.position;
                RemoveCursor();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (playerMove.FinishedMovement && fadeImages[5] != 1 && canAttack)
            {
                fadeImages[5] = 1;
                anim.SetInteger("Atk", 6);
                playerMove.TargetPosition = transform.position;
                RemoveCursor();
            }
        }
        else
        {
            anim.SetInteger("Atk", 0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 targetPos = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(targetPos - transform.position), playerMove.RotationSpeed * Time.deltaTime);
        }
    } // check input

    #endregion

    #region CheckToFade

    void CheckToFade()
    {
        if (fadeImages[0] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage1, 1.0f))
                fadeImages[0] = 0;
        }
        if (fadeImages[1] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage2, 0.7f))
                fadeImages[1] = 0;
        }
        if (fadeImages[2] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage3, 0.1f))
                fadeImages[2] = 0;
        }
        if (fadeImages[3] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage4, 0.2f))
                fadeImages[3] = 0;
        }
        if (fadeImages[4] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage5, 0.3f))
                fadeImages[4] = 0;
        }
        if (fadeImages[5] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage6, 0.08f))
                fadeImages[5] = 0;
        }
    }

    #endregion

    #region FadeAndWait

    bool FadeAndWait(Image fadeImg, float fadeTime)
    {
        bool faded = false;
        if (fadeImg == null)
            return faded;

        if (!fadeImg.gameObject.activeInHierarchy)
        {
            fadeImg.gameObject.SetActive(true);
            fadeImg.fillAmount = 1f;
        }
        fadeImg.fillAmount -= fadeTime * Time.deltaTime;

        if (fadeImg.fillAmount <= 0)
        {
            fadeImg.gameObject.SetActive(false);
            faded = true;
        }
        return faded;
    }

    #endregion

    private void RemoveCursor()
    {
        var cursor = GameObject.FindGameObjectWithTag("Cursor");
        if (cursor)
            Destroy(cursor);
    }
}