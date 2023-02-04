using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 10f;
        public Camera perspectiveCamera;
        private CharacterController _characterController;
        private float _interactInput;
        private float _jumpInput;
        private Vector2 _moveInput;
        private Transform _perspectiveCameraTransform;
        private Vector3 _perspectiveForward;
        private Vector3 _perspectiveRight;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _characterController = GetComponent<CharacterController>();
            perspectiveCamera ??= Camera.main;
            if (perspectiveCamera)
                _perspectiveCameraTransform = perspectiveCamera.transform;
        }
        private void Update()
        {
            if (_moveInput != Vector2.zero)
            {
                // Note: May have to move this when the player first starts joystick input
                // because may have cinemachine
                ReadPerspectiveCamera();
                var desiredMoveDirection = _perspectiveForward * _moveInput.y + _perspectiveRight * _moveInput.x;
                _characterController.Move(desiredMoveDirection * moveSpeed * Time.deltaTime);
            }
        }
        private void ReadPerspectiveCamera()
        {
            _perspectiveForward = _perspectiveCameraTransform.forward;
            _perspectiveRight = _perspectiveCameraTransform.right;
            NormalizeReadPerspectiveCameraValues();
        }
        private void NormalizeReadPerspectiveCameraValues()
        {
            _perspectiveForward.y = 0f;
            _perspectiveRight.y = 0f;
            _perspectiveForward.Normalize();
            _perspectiveRight.Normalize();
        }

        private void OnMove(InputValue value)
        {
            _moveInput = value.Get<Vector2>();
            Debug.Log($"on move {_moveInput}");
        }

        private void OnInteract(InputValue value)
        {
            _interactInput = value.Get<float>();
            Debug.Log($"on interact {_interactInput}");
        }

        private void OnJump(InputValue value)
        {
            _jumpInput = value.Get<float>();
            Debug.Log($"on jump {_jumpInput}");
        }
    }
}
