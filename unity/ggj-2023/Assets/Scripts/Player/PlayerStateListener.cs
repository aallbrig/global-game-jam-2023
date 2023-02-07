using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerStateListener : MonoBehaviour
    {
        public PlayerState listenState;
        public UnityEvent onState;
        public PlayerController targetPlayerStateMachine;
        private void Awake() => targetPlayerStateMachine ??= GetComponent<PlayerController>();
        private void OnEnable() => targetPlayerStateMachine.onStateTransition.AddListener(OnPlayerStateMachineTransition);
        private void OnDisable() => targetPlayerStateMachine.onStateTransition.RemoveListener(OnPlayerStateMachineTransition);
        private void OnPlayerStateMachineTransition(PlayerState previousState, PlayerState currentState)
        {
            if (currentState == listenState) onState?.Invoke();
        }
    }
}