using System;
using CleverCrow.Fluid.FSMs;
using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public enum TreeState
    {
        Seed,
        Sprout,
        Sapling,
        Mature
    }

    public class Tree : MonoBehaviour, IStateMachineFactory, IInteractable
    {
        public TreeState startTreeState;
        public UnityEvent onSprout;
        public UnityEvent onSapling;
        public UnityEvent onMature;

        [SerializeField] private TreeState currentTreeState;

        private bool _timeToGrow;
        private Transform _transform;
        private IFsm _treeStateMachine;
        private void Awake()
        {
            _transform = transform;
            _treeStateMachine = Build();
        }
        private void Update()
        {
            _treeStateMachine.Tick();
            var previousTreeState = currentTreeState;
            currentTreeState = (TreeState)_treeStateMachine.CurrentState.Id;
            if (previousTreeState != currentTreeState)
                switch (currentTreeState)
                {
                    case TreeState.Sprout:
                        onSprout?.Invoke();
                        break;
                    case TreeState.Sapling:
                        onSapling?.Invoke();
                        break;
                    case TreeState.Mature:
                        onMature?.Invoke();
                        break;
                }
        }
        public void Interact() => GrowYourRoots();
        public IFsm Build() => new FsmBuilder()
            .Owner(gameObject)
            .Default(startTreeState)
            .State(TreeState.Seed, stateBuilder =>
                stateBuilder
                    .SetTransition(TreeState.Sprout.ToString(), TreeState.Sprout)
                    .Enter(_ =>
                    {
                        _transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    })
                    .Update(GenerateUpdateFunction(TreeState.Sprout.ToString())))
            .State(TreeState.Sprout, stateBuilder =>
                stateBuilder
                    .SetTransition(TreeState.Sapling.ToString(), TreeState.Sapling)
                    .Enter(_ =>
                    {
                        _transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    })
                    .Update(GenerateUpdateFunction(TreeState.Sapling.ToString())))
            .State(TreeState.Sapling, stateBuilder =>
                stateBuilder
                    .SetTransition(TreeState.Mature.ToString(), TreeState.Mature)
                    .Enter(_ =>
                    {
                        _transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    })
                    .Update(GenerateUpdateFunction(TreeState.Mature.ToString())))
            .State(TreeState.Mature, stateBuilder =>
                stateBuilder
                    .Enter(_ =>
                    {
                        _transform.localScale = new Vector3(1f, 1f, 1f);
                    }))
            .Build();
        private Action<IAction> GenerateUpdateFunction(string nextState) => action =>
        {
            if (_timeToGrow)
            {
                _timeToGrow = false;
                action.Transition(nextState);
            }
        };
        [ContextMenu("Grow Your Roots")] public void GrowYourRoots() => _timeToGrow = true;
    }
}
