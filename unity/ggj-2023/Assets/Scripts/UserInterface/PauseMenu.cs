using System;
using CleverCrow.Fluid.FSMs;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UserInterface
{
    public enum PauseMenuState
    {
        Hidden,
        Active
    }

    [RequireComponent(typeof(UIDocument))]
    public class PauseMenu : MonoBehaviour, IStateMachineFactory
    {
        public PauseMenuState startMenuState;
        public PauseMenuState currentMenuState;
        private bool _buttonConsumed;
        private float _pauseInput;
        private IFsm _pauseMenuStateMachine;
        private UIDocument _uiDocument;
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _pauseMenuStateMachine = Build();
        }
        private void Update() => _pauseMenuStateMachine.Tick();
        public IFsm Build() => new FsmBuilder()
            .Owner(gameObject)
            .Default(startMenuState)
            .State(PauseMenuState.Active, stateBuilder =>
            {
                stateBuilder
                    .SetTransition(PauseMenuState.Hidden.ToString(), PauseMenuState.Hidden)
                    .Enter(_ => _uiDocument.enabled = true)
                    .Update(action =>
                    {
                        if (_pauseInput > 0f && _buttonConsumed == false)
                        {
                            _buttonConsumed = true;
                            action.Transition(PauseMenuState.Hidden.ToString());
                        }
                    });
            })
            .State(PauseMenuState.Hidden, stateBuilder =>
            {
                stateBuilder
                    .SetTransition(PauseMenuState.Active.ToString(), PauseMenuState.Active)
                    .Enter(_ => _uiDocument.enabled = false)
                    .Update(action =>
                    {
                        if (_pauseInput > 0f && _buttonConsumed == false)
                        {
                            _buttonConsumed = true;
                            action.Transition(PauseMenuState.Active.ToString());
                        }
                    });
            })
            .Build();
        private void OnPause(InputValue value)
        {
            var newValue = value.Get<float>();
            if (Math.Abs(newValue - _pauseInput) > 0.1f && newValue > 0) _buttonConsumed = false;
            _pauseInput = newValue;
            Debug.Log($"{name} | on pause {_pauseInput}, _buttonConsumed {_buttonConsumed}");
        }
    }
}
