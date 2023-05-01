using UnityEngine;

namespace Assets.__Game.Scripts.Level {
    public sealed class ObstacleHandler : MonoBehaviour {
        [SerializeField] private Transform collectiblePoint;
        [SerializeField] private GameObject[] collectibles;

        private void Start() {
            SpawnCollectible();
        }

        private void SpawnCollectible() {
            var randSpawn = Random.Range(0, 2);

            if (randSpawn == 0)
                Instantiate(collectibles[Random.Range(0, collectibles.Length)],
                    collectiblePoint.position, Quaternion.identity, transform);
        }
    }
}