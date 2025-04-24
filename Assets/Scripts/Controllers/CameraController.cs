using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float _minHeight = 10f;
    public float _maxHeight = 40f;

    [Header("Movement Bounds")]
    public Vector2 _minBounds = new(-500, -500);
    public Vector2 _maxBounds = new(500, 500);

    [Header("Movement Settings")]
    public float _moveSpeed = 2000f;
    public float _zoomSpeed = 6f;
    private float _defaultY;

    void Awake()
    {
        _defaultY = transform.position.y;
    }


    private void OnEnable()
    {
        InputManager.onSwiped += Move;
        InputManager.onZoomed += Zoom;
    }

    private void OnDisable()
    {
        InputManager.onSwiped -= Move;
        InputManager.onZoomed -= Zoom;
    }

    private void Move(Vector2 inputDelta)
    {
        Vector3 horizontalDir = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
        Vector3 verticalDir = Vector3.ProjectOnPlane(transform.up, Vector3.up).normalized;


        float speed = _moveSpeed * transform.position.y / _defaultY;
        float multiplayer = speed * Time.deltaTime / Screen.height;
        float deltaHorizontal = inputDelta.x * multiplayer;
        float deltaVertical = inputDelta.y * multiplayer;

        Vector3 newPos = transform.position - horizontalDir * deltaHorizontal - verticalDir * deltaVertical;

        newPos.x = Mathf.Clamp(newPos.x, _minBounds.x, _maxBounds.x);
        newPos.z = Mathf.Clamp(newPos.z, _minBounds.y, _maxBounds.y);
        transform.position = newPos;
    }

    private void Zoom(Vector2 inputCentre, float inputDelta)
    {
        // Convert inputCentre from screen space to world space
        Ray ray = Camera.main.ScreenPointToRay(inputCentre);
        Vector3 targetPoint = transform.position + ray.direction * 100f;
        // Calculate the zoom factor
        float zoomFactor = Mathf.Clamp(transform.position.y - inputDelta * _zoomSpeed * Time.deltaTime, _minHeight, _maxHeight) / transform.position.y;
        Debug.Log(inputDelta * _zoomSpeed * Time.deltaTime);

        // Adjust the camera position
        Vector3 direction = transform.position - targetPoint;
        Vector3 newPos = targetPoint + direction * zoomFactor;

        // Clamp the new position
        newPos.y = Mathf.Clamp(newPos.y, _minHeight, _maxHeight);
        newPos.x = Mathf.Clamp(newPos.x, _minBounds.x, _maxBounds.x);
        newPos.z = Mathf.Clamp(newPos.z, _minBounds.y, _maxBounds.y);

        transform.position = newPos;
    }
}