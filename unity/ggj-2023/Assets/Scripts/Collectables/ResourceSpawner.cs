using UnityEngine;

namespace Collectables
{
    public class ResourceSpawner : MonoBehaviour
    {
        public GameObject waterResourcePrefab;
        public GameObject soilResourcePrefab;
        public GameObject sunlightResourcePrefab;
        public int waterAmount = 1;
        public int soilAmount = 1;
        public int sunlightAmount = 1;
        public float radius = 10f;
        public float minHeight = 8f;
        public float maxHeight = 16f;
        public void SpawnResources()
        {
            SpawnResource(waterResourcePrefab, waterAmount);
            SpawnResource(soilResourcePrefab, soilAmount);
            SpawnResource(sunlightResourcePrefab, sunlightAmount);
        }
        private void SpawnResource(GameObject resourcePrefab, int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var resource = Instantiate(resourcePrefab);
                resource.transform.position = transform.position +
                                              new Vector3(Random.Range(-radius, radius), Random.Range(minHeight, maxHeight),
                                                  Random.Range(-radius, radius));
                if (resource.TryGetComponent<Rigidbody>(out var resourceRigidbody))
                    resourceRigidbody.AddForce(new Vector3(0, 1, 0));
            }
        }
    }
}
