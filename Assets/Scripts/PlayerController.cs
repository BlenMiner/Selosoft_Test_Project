using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PipePath _path;

    [Header("Camera settings")]
    [SerializeField] Transform _camera;
    [SerializeField] Vector3 _cameraOffset;

    [Header("Move settings")]
    [SerializeField] float _unitsPerSecond = 2f;
    [SerializeField] float _mouseDragSpeed = 1f;

    private int _currentPipe = 0;
    private Vector3 _playerPosition;
    private Vector3 _lastMousePosition;
    private Quaternion _playerRotation;

    private float _horizontal = 0.001f;

    private void Start()
    {
        // Initialize some variables
        _playerPosition = _path.SpawnPosition;

        transform.position = _playerPosition;

        _lastMousePosition = Input.mousePosition;

        _playerRotation = transform.rotation;
    }

    private float Move(float maxDelta)
    {
        var pipe = _path.GetPipe(_currentPipe);

        // Move pipes if we already at the end of current one
        if (pipe.EndPosition == _playerPosition)
        {
            // If we reached the end just return
            if (_path.EndOfPipes(_currentPipe + 1))
                return maxDelta;

            pipe = _path.GetPipe(++_currentPipe);
        }

        Vector3 deltaMove = pipe.EndPosition - _playerPosition;

        if (deltaMove.magnitude > maxDelta)
            deltaMove = deltaMove.normalized * maxDelta;

        _playerPosition += deltaMove;

        return deltaMove.magnitude;
    }

    private void SimpleMove()
    {
        float maxDelta = _unitsPerSecond * Time.deltaTime;
        float moved = 0f;

        // Make sure we move the 2 units per second
        while (maxDelta - moved > 0f)
            moved += Move(maxDelta - moved);
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            var pipe = _path.GetPipe(_currentPipe);

            float halfWidth = pipe.Width * 0.5f;

            Vector3 delta = Input.mousePosition - _lastMousePosition;

            _horizontal += delta.x * Time.deltaTime * _mouseDragSpeed;
            _horizontal = Mathf.Clamp(_horizontal, -halfWidth, halfWidth);
        }

        _lastMousePosition = Input.mousePosition;
    }

    private void CalculatePlayerPosition()
    {
        var pipe = _path.GetPipe(_currentPipe);

        Vector3 pipeDirection = (pipe.StartPosition - pipe.EndPosition).normalized;

        Vector3 right = Vector3.Cross(pipeDirection, Vector3.up);

        Vector3 targetPlayerPosition = _playerPosition + right * _horizontal;

        if (Physics.Raycast(new Ray(targetPlayerPosition, Vector2.down), out RaycastHit hit))
        {
            var rotation = Quaternion.LookRotation(transform.forward, hit.normal);
            _playerRotation = Quaternion.Lerp(_playerRotation, rotation, Time.deltaTime * 20f);

            transform.rotation = _playerRotation;
            transform.position = hit.point;
        }
    }

    private void CalculateCameraPosition()
    {
        Vector3 cmrOffset = transform.TransformVector(new Vector3(_cameraOffset.x, 0, _cameraOffset.z));

        cmrOffset.y = _cameraOffset.y;

        _camera.transform.position = _playerPosition + cmrOffset;
    }

    private void Update()
    {
        HandleInput();

        SimpleMove();

        CalculatePlayerPosition();

        CalculateCameraPosition();
    }
}
