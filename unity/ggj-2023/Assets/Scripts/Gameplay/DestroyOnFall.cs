using UnityEngine;

namespace Gameplay
{
    public class DestroyOnFall : MonoBehaviour
    {
        public float floor = -20f;
        private Transform _transform;
        private void Awake() => _transform = transform;
        private void Update()
        {
            if (_transform.position.y <= floor) Destroy(gameObject);
        }
    }
}
