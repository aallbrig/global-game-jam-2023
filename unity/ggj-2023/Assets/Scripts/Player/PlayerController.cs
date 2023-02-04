using CleverCrow.Fluid.FSMs;
using Core;
using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public enum PlayerState
    {
        Exploring,
        Jumping,
        Interacting
    }

    [RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, IStateMachineFactory
    {
        public float moveSpeed = 10f;
        public float jumpMoveSpeed = 4f;
        public float jumpHeight = 2f;
        public float interactCooldown = 1f;
        public Camera perspectiveCamera;
        public PlayerState startPlayerState;
        public Transform playerAvatar;

        [SerializeField] private PlayerState currentPlayerState;

        private CharacterController _characterController;
        private IInteractable _interactable;
        private float _interactInput;
        private float _interactTime;
        private float _jumpInput;
        private Vector2 _moveInput;
        private Transform _perspectiveCameraTransform;
        private Vector3 _perspectiveForward;
        private Vector3 _perspectiveRight;
        private IFsm _playerStateMachine;
        private PlayerState _previousStateBeforeInteract;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            perspectiveCamera ??= Camera.main;
            if (perspectiveCamera)
                _perspectiveCameraTransform = perspectiveCamera.transform;
            _playerStateMachine = Build();
        }

        private void Update() => _playerStateMachine.Tick();
        private void OnTriggerEnter(Collider other)
        {
            if (_interactable == null && other.TryGetComponent<IInteractable>(out var interactable))
                _interactable = interactable;
        }
        private void OnTriggerExit(Collider other)
        {
            if (_interactable != null && other.TryGetComponent<IInteractable>(out var interactable) &&
                _interactable == interactable)
                _interactable = null;
        }
        public IFsm Build() => new FsmBuilder()
            .Owner(gameObject)
            .Default(startPlayerState)
            .State(PlayerState.Exploring, stateBuilder =>
            {
                stateBuilder
                    .SetTransition($"{PlayerState.Jumping}", PlayerState.Jumping)
                    .Enter(action =>
                    {
                        currentPlayerState = PlayerState.Exploring;
                    })
                    .Update(action =>
                    {
                        var desiredMove = Vector3.zero;
                        if (_moveInput != Vector2.zero)
                        {
                            // Note: May have to move this when the player first starts joystick input
                            // because may have cinemachine
                            ReadPerspectiveCamera();
                            desiredMove += _perspectiveForward * _moveInput.y + _perspectiveRight * _moveInput.x;
                            // _characterController.Move(desiredMoveDirection * moveSpeed * Time.deltaTime);
                        }
                        if (_jumpInput > 0f)
                            action.Transition($"{PlayerState.Jumping}");
                        if (_interactable != null && _interactInput > 0f && Time.time - _interactTime > interactCooldown)
                        {
                            _interactTime = Time.time;
                            _interactable.Interact();
                        }
                        // Jump
                        // desiredMove += new Vector3(0, 10f, 0);
                        if (desiredMove != Vector3.zero)
                            _characterController.SimpleMove(desiredMove * moveSpeed);
                    });
            })
            .State(PlayerState.Jumping, stateBuilder =>
            {
                stateBuilder
                    .SetTransition($"{PlayerState.Exploring}", PlayerState.Exploring)
                    .Enter(action =>
                    {
                        currentPlayerState = PlayerState.Jumping;
                        _characterController.Move(new Vector3(0, 1f, 0) * jumpHeight);
                    })
                    .Update(action =>
                    {
                        if (_characterController.isGrounded)
                        {
                            action.Transition($"{PlayerState.Exploring}");
                            return;
                        }
                        var desiredMove = Vector3.zero;
                        if (_moveInput != Vector2.zero)
                        {
                            // Note: May have to move this when the player first starts joystick input
                            // because may have cinemachine
                            ReadPerspectiveCamera();
                            desiredMove += _perspectiveForward * _moveInput.y + _perspectiveRight * _moveInput.x;
                        }
                        _characterController.SimpleMove(desiredMove * jumpMoveSpeed);
                    });
            })
            .State(PlayerState.Interacting, stateBuilder =>
            {
                stateBuilder
                    .Enter(action =>
                    {
                        _previousStateBeforeInteract = currentPlayerState;
                        currentPlayerState = PlayerState.Interacting;
                    });
            })
            .Build();

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
