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
                var forward = _perspectiveCameraTransform.forward;
                var right = _perspectiveCameraTransform.right;
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();
                var desiredMoveDirection = forward * _moveInput.y + right * _moveInput.x;
                _characterController.Move(desiredMoveDirection * moveSpeed * Time.deltaTime);
            }
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
