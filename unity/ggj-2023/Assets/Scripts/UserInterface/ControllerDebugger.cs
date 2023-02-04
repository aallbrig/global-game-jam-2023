using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UserInterface
{
    [RequireComponent(typeof(PlayerInput), typeof(UIDocument))]
    public class ControllerDebugger : MonoBehaviour
    {
        private Label _controllerName;
        private Label _controlScheme;
        private Label _deviceInfo;
        private Label _interact;
        private Label _jump;
        private Label _look;
        private Label _lookButton;
        private Label _movement;
        private Label _pause;
        private PlayerInput _playerInput;
        private UIDocument _uiDocument;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _uiDocument = GetComponent<UIDocument>();
            _controllerName = _uiDocument.rootVisualElement.Q<Label>("controllerName");
            _controlScheme = _uiDocument.rootVisualElement.Q<Label>("controlScheme");
            _deviceInfo = _uiDocument.rootVisualElement.Q<Label>("deviceInfo");
            _movement = _uiDocument.rootVisualElement.Q<Label>("movement");
            _look = _uiDocument.rootVisualElement.Q<Label>("look");
            _lookButton = _uiDocument.rootVisualElement.Q<Label>("lookButton");
            _interact = _uiDocument.rootVisualElement.Q<Label>("interact");
            _jump = _uiDocument.rootVisualElement.Q<Label>("jump");
            _pause = _uiDocument.rootVisualElement.Q<Label>("pause");
        }

        private void Start()
        {
            // Find current input device and display its name
            // _controllerName
            // What is the current control scheme?
            _controllerName.text = $"{_playerInput.currentControlScheme}";
            _controlScheme.text = $"{_playerInput.currentControlScheme}";
            _deviceInfo.text = $"device count: {_playerInput.devices.Count}";
        }

        private void OnMove(InputValue value) => _movement.text = $"{value.Get<Vector2>()}";
        private void OnLook(InputValue value) => _look.text = $"{value.Get<Vector2>()}";
        private void OnLookReset(InputValue value) => _lookButton.text = $"{value.Get<float>()}";
        private void OnInteract(InputValue value) => _interact.text = $"{value.Get<float>()}";
        private void OnJump(InputValue value) => _jump.text = $"{value.Get<float>()}";
        private void OnPause(InputValue value) => _pause.text = $"{value.Get<float>()}";
    }
}
