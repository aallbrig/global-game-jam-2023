using UnityEngine;
using UnityEngine.Events;

namespace Collectables
{
    public class Collectable : MonoBehaviour, ICollectable
    {
        public UnityEvent onCollect;
        public void Collect() => onCollect?.Invoke();
    }
}
