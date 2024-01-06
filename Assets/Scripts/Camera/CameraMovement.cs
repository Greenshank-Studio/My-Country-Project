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

    private bool _isMenuOpened;

   private void OnEnable() 
    {
       UIController.OnMenuStateChanged += ChangeCameraMovementState;
    }

    private void OnDisable() 
    {
        UIController.OnMenuStateChanged -= ChangeCameraMovementState;
    }

    
    private void Awake()
    {
        _normalSpeed = 0.1f;
        _fastSpeed = 0.3f;
        _movementSpeed = _normalSpeed;
        _movementTime = 3f;
        _zoomAmount = 0.5f;

        _rigTransform = GetComponent<Transform>();
        _cameraComponent = _rigTransform.GetChild(0).GetComponent<Camera>();

        _newPosition = _rigTransform.position;
    }

    private void LateUpdate()
    {
        if(!_isMenuOpened)
        {
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
        if (Input.mouseScrollDelta.y > 0 && _cameraComponent.orthographicSize > 1.5)
        {
            _cameraComponent.orthographicSize -= Input.mouseScrollDelta.y * _zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0 && _cameraComponent.orthographicSize < 15)
        {
            _cameraComponent.orthographicSize -= Input.mouseScrollDelta.y * _zoomAmount;
        }
    }

    private void ChangeCameraMovementState(bool isMenuOpened)
    {
        _isMenuOpened = isMenuOpened;
    }


}

