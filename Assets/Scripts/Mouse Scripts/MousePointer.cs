using UnityEngine;

public class MousePointer : MonoBehaviour
{
    [SerializeField] Texture2D cursorTexture;

    [SerializeField] GameObject _mousePointer;
    GameObject _instantiateMouse;
    private CursorMode mode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero;


    void Start()
    {
    }

    void Update()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, mode);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider is TerrainCollider)
                {
                    Vector3 tempPos = hit.point;
                    tempPos.y = 0.25f;
                    if (_instantiateMouse == null)
                    {
                        _instantiateMouse = Instantiate(_mousePointer) as GameObject;
                        _instantiateMouse.transform.position = tempPos;
                    }
                    else
                    {
                        Destroy(_instantiateMouse);
                        _instantiateMouse = Instantiate(_mousePointer) as GameObject;
                        _instantiateMouse.transform.position = tempPos;
                    }
                }
            }
        }
    }
}