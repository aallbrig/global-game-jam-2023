using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Graphs;
using Gameplay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Dialogue
{
    [RequireComponent(typeof(UIDocument), typeof(AudioSource))]
    public class Speaker : MonoBehaviour, IInteractable
    {
        public DialogueGraph dialogueGraph;
        public UnityEvent onDialogueStart;
        public UnityEvent onDialogueSpeak;
        public UnityEvent onDialogueEnd;
        private AudioSource _audioSource;
        private DialogueController _dialogueController;
        private Label _dialogueText;
        private Label _speakerName;
        private bool _speaking;
        private UIDocument _uiDocument;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _uiDocument = GetComponent<UIDocument>();
            SyncUiDocument();
            _dialogueController = new DialogueController(new DatabaseInstanceExtended());
            _dialogueController.Events.Begin.AddListener(() =>
            {
                _speaking = true;
                SyncUiDocument();
                Debug.Log($"{name} | begin | (speaking {_speaking})");
                onDialogueStart?.Invoke();
            });
            _dialogueController.Events.End.AddListener(() =>
            {
                _speaking = false;
                SyncUiDocument();
                Debug.Log($"{name} | end | (speaking {_speaking})");
                onDialogueEnd?.Invoke();
            });
            _dialogueController.Events.SpeakWithAudio.AddListener((actor, text, clip) =>
            {
                if (clip)
                {
                    _audioSource.clip = clip;
                    _audioSource.Play();
                }
                Debug.Log($"{name} | speak | {actor} {text}");
                _speakerName.text = actor.DisplayName;
                _dialogueText.text = text;
                onDialogueSpeak?.Invoke();
            });
            _dialogueController.Events.NodeEnter.AddListener(node =>
            {
                Debug.Log($"{name} | node enter | {node}");
            });
        }

        public void Interact(GameObject interactor)
        {
            if (_speaking == false) _dialogueController.Play(dialogueGraph);
            else _dialogueController.Next();
        }

        private void SyncUiDocument()
        {
            if (_uiDocument.enabled != _speaking) _uiDocument.enabled = _speaking;
            if (_uiDocument.enabled)
            {
                _dialogueText = _uiDocument.rootVisualElement.Q<Label>("dialogueTextDisplay");
                _speakerName = _uiDocument.rootVisualElement.Q<Label>("speakerName");
            }
        }
    }
}
