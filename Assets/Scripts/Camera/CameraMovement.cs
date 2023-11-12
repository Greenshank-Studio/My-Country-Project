using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float _normalSpeed;
    private float _fastSpeed;
    private float _movementSpeed;
    private float _movementTime;
    private float _zoomAmount;

    private Vector3 _dragStartPosition;
    private Vector3 _dragCurrentPosition;

    private Transform _rigTransform;
    private Camera _cameraComponent;

    private Vector3 _newPosition;

    private bool _is_camera_moving;

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
    Debug.Log("Applying high CPU load workaround");
    UnityEditor.EditorApplication.update += () => {
        System.Threading.Thread.Sleep(10);
    };
    }

    private void Awake()
    {
        _normalSpeed = 10.1f;
        _fastSpeed = 20.9f;
        _movementSpeed = _normalSpeed;
        _movementTime = 3f;
        _zoomAmount = 0.5f; // zoom speed
        _is_camera_moving = true;

        _rigTransform = GetComponent<Transform>();
        _cameraComponent = Camera.main; // _rigTransform.GetChild(0).GetComponent<Camera>();

        Debug.Log(Camera.main == null);

        _newPosition = _rigTransform.position;
    }

    private void Start(){
        Application.targetFrameRate = 60; // set frame rate to 30 fps
    }

    private void OnEnable(){ // whan current object is enabled (set active == true)
        Menu.onMenuOpen += StopCameraMovement;
    }

    private void OnDisable(){
        Menu.onMenuOpen -= StopCameraMovement;
    }

    private void StopCameraMovement(bool is_camera_moving){
        _is_camera_moving = is_camera_moving;
    }

    private void LateUpdate() // build-in method
    {
        if (_is_camera_moving){
            HandleAllKeyboardInput();
            HandleAllMouseInput();
        }
    }

    private void HandleAllKeyboardInput()
    {
        HandleAccelerationInput();
        HandleMovementInput();
    }

    private void HandleMovementInput() 
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _newPosition += _rigTransform.forward * _movementSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _newPosition += _rigTransform.forward * -_movementSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _newPosition += _rigTransform.right * _movementSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _newPosition += _rigTransform.right * -_movementSpeed;
        }

        if (_newPosition.x > 55) _newPosition.x = 55;
        if (_newPosition.x < -5) _newPosition.x = -5;
        if (_newPosition.z > 55) _newPosition.z = 55;
        if (_newPosition.z < -5) _newPosition.z = -5;
        _rigTransform.position = Vector3.Lerp(_rigTransform.position, _newPosition, Time.deltaTime * _movementTime);
    }

    private void HandleAccelerationInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _movementSpeed = _fastSpeed;
        }
        else
        {
            _movementSpeed = _normalSpeed;
        }
    }

    private void HandleAllMouseInput()
    {
        HandleMouseMovementInput();
        HandleMouseZoomInput();
    }

    private void HandleMouseMovementInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float enter))
            {
                _dragStartPosition = ray.GetPoint(enter);
            }
            
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float enter))
            {
                _dragCurrentPosition = ray.GetPoint(enter);

                _newPosition = _rigTransform.position + _dragStartPosition - _dragCurrentPosition;
            }
        }
        
    }

    private void HandleMouseZoomInput()
    {
        if (Input.mouseScrollDelta.y > 0 && _cameraComponent.orthographicSize > 3)
        {
            _cameraComponent.orthographicSize -= Input.mouseScrollDelta.y * _zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0 && _cameraComponent.orthographicSize < 9)
        {
            _cameraComponent.orthographicSize -= Input.mouseScrollDelta.y * _zoomAmount;
        }
    }
}
