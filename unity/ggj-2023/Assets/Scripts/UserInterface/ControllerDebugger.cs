using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UserInterface
{
    [RequireComponent(typeof(PlayerInput), typeof(UIDocument))]
    public class ControllerDebugger : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private PlayerInput _playerInput;
        private Label _controllerInput;
        private Label _controllerName;
        private Label _deviceInfo;
        private Label _movement;
        private Label _interact;
        private Label _jump;
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _uiDocument = GetComponent<UIDocument>();
            _controllerInput = _uiDocument.rootVisualElement.Q<Label>("controllerInput");
            _controllerName = _uiDocument.rootVisualElement.Q<Label>("controllerName");
            _deviceInfo = _uiDocument.rootVisualElement.Q<Label>("deviceInfo");
            _movement = _uiDocument.rootVisualElement.Q<Label>("movement");
            _interact = _uiDocument.rootVisualElement.Q<Label>("interact");
            _jump = _uiDocument.rootVisualElement.Q<Label>("jump");
            _controllerName.text = $"{_playerInput.currentControlScheme}";
            _controllerInput.text = $"value: {_playerInput.inputIsActive}";
        }
        private void OnMove(InputValue value)
        {
            _movement.text = $"{value.Get<Vector2>()}";
        }
        private void OnInteract(InputValue value)
        {
            _interact.text = $"{value.Get<float>()}";
        }
        private void OnJump(InputValue value)
        {
            _jump.text = $"{value.Get<float>()}";
        }
    }
}
