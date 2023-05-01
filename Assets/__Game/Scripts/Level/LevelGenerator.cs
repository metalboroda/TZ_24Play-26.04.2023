using __Game.Scripts.Player;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.__Game.Scripts.Level {
    public class LevelGenerator : MonoBehaviour {
        public static LevelGenerator Instance;

        [field: SerializeField] public float SpawnOffset { get; private set; }
        [SerializeField] private GameObject[] tracksToSpawn;
        [SerializeField] private List<GameObject> spawnedTracks = new();

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            PlayerHandler.Instance.OnPassedDistance += OnPassedDistance;

            SpawnTrack();
        }

        private void OnPassedDistance() {
            SpawnTrack();
            RemoveFirstTrack();
            ResetPosition();
        }

        private void SpawnTrack() {
            var spawnPosition = new Vector3(0, -50f, spawnedTracks.Last().transform.position.z + SpawnOffset);
            var spawnRotation = Quaternion.Euler(90, 0, 0);

            var spawnedTrack = Instantiate(tracksToSpawn[Random.Range(0, tracksToSpawn.Length)],
                spawnPosition, spawnRotation, transform);

            spawnedTrack.transform.DOLocalMoveY(-1.5f, 0.25f);

            spawnedTracks.Add(spawnedTrack);
        }

        private void RemoveFirstTrack() {
            Destroy(spawnedTracks.First());
            spawnedTracks.Remove(spawnedTracks.First());
        }

        private void ResetPosition() {
            foreach (var i in spawnedTracks) {
                var newPos = new Vector3(0, i.transform.localPosition.y, i.transform.localPosition.z - SpawnOffset);
                i.transform.position = newPos;
            }
        }
    }
}