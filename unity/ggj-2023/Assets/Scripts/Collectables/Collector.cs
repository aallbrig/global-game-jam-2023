using UnityEngine;

namespace Collectables
{
    public class Collector : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Collectable>(out var collectable))
                Destroy(collectable.gameObject);
        }
    }
}
