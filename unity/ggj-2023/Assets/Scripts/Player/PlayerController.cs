using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 10f;
        public Camera perspectiveCamera;
        private PlayerInput _playerInput;
        private CharacterController _characterController;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _characterController = GetComponent<CharacterController>();
            perspectiveCamera ??= Camera.main;
        }
        private void OnMove(InputValue value)
        {
            Debug.Log($"on move {value.Get<Vector2>()}");
        }
        private void OnInteract(InputValue value)
        {
            Debug.Log($"on interact {value.Get<float>()}");
        }
        private void OnJump(InputValue value)
        {
            Debug.Log($"on jump {value.Get<float>()}");
        }
    }
}
