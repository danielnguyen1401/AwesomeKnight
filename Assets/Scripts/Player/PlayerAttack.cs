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

    readonly int[] _fadeImages = new int[] {0, 0, 0, 0, 0, 0};
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerMove _playerMove;
    private bool _canAttack = true;

    void Awake()
    {
//        anim = GetComponent<Animator>();
//        _playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
            _canAttack = true;
        else
            _canAttack = false;

        CheckToFade();
        CheckInput();
    }

    #region CheckInput

    void CheckInput()
    {
        if (anim.GetInteger("Atk") == 0)
        {
            _playerMove.FinishedMovement = false;

            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                _playerMove.FinishedMovement = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Ground Impact
        {
            _playerMove.TargetPosition = transform.position;

            if (_playerMove.FinishedMovement && _fadeImages[0] != 1 && _canAttack)
            {
                _fadeImages[0] = 1;
                anim.SetInteger("Atk", 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Kick
        {
            _playerMove.TargetPosition = transform.position;

            if (_playerMove.FinishedMovement && _fadeImages[1] != 1 && _canAttack)
            {
                _fadeImages[1] = 1;
                anim.SetInteger("Atk", 2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Fire Tornado
        {
            _playerMove.TargetPosition = transform.position;

            if (_playerMove.FinishedMovement && _fadeImages[2] != 1 && _canAttack)
            {
                _fadeImages[2] = 1;
                anim.SetInteger("Atk", 3); 
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // Fire Shield
        {
            _playerMove.TargetPosition = transform.position;

            if (_playerMove.FinishedMovement && _fadeImages[3] != 1 && _canAttack)
            {
                _fadeImages[3] = 1;
                anim.SetInteger("Atk", 4);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) // Health
        {
            _playerMove.TargetPosition = transform.position;

            if (_playerMove.FinishedMovement && _fadeImages[4] != 1 && _canAttack)
            {
                _fadeImages[4] = 1;
                anim.SetInteger("Atk", 5);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _playerMove.TargetPosition = transform.position;

            if (_playerMove.FinishedMovement && _fadeImages[5] != 1 && _canAttack)
            {
                _fadeImages[5] = 1;
                anim.SetInteger("Atk", 6);
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

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), _playerMove.RotationSpeed * Time.deltaTime);
        }
    } // check input

    #endregion

    #region CheckToFade

    void CheckToFade()
    {
        if (_fadeImages[0] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage1, 1.0f))
                _fadeImages[0] = 0;
        }
        if (_fadeImages[1] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage2, 0.7f))
                _fadeImages[1] = 0;
        }
        if (_fadeImages[2] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage3, 0.1f))
                _fadeImages[2] = 0;
        }
        if (_fadeImages[3] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage4, 0.2f))
                _fadeImages[3] = 0;
        }
        if (_fadeImages[4] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage5, 0.3f))
                _fadeImages[4] = 0;
        }
        if (_fadeImages[5] == 1)
        {
            if (FadeAndWait(_fillWaitImgImage6, 0.08f))
                _fadeImages[5] = 0;
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
}