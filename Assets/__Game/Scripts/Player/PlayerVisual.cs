using __Game.Scripts.Player;
using Assets.__Game.Scripts.Level;
using DG.Tweening;
using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.__Game.Scripts.Player {
    public class PlayerVisual : MonoBehaviour {

        [SerializeField] private GameObject warpVFX;
        [SerializeField] private GameObject stackTextVFX;
        [SerializeField] private GameObject maxCapacityTextVFX;

        [Header("Trail")]
        public LineRenderer lineRenderer;
        public List<Vector3> points = new();
        public int maxPoints = 50;

        //Private
        private StickmanCollider stickmanCollider;

        private void Awake() {
            stickmanCollider = GetComponentInChildren<StickmanCollider>();
        }

        private void Start() {
            PlayerHandler.Instance.OnPlayerStatedUpdate += Warp;
            PickableHolderHandler.Instance.OnPickableStacking += StackTextShow;
            PickableHolderHandler.Instance.OnMaxCapacityReached += MaxCapacityTextShow;
            PlayerHandler.Instance.OnPassedDistance += ResetLineRendererZ;
        }

        private void Update() {
            MakeTrail();
        }

        private void Warp(PlayerState state) {
            if (state == PlayerState.Move) {
                warpVFX.SetActive(true);
            } else
                warpVFX.SetActive(false);
        }

        private void StackTextShow() {
            var spawnPos = new Vector3(stickmanCollider.transform.position.x, stickmanCollider.transform.position.y + 1,
                stickmanCollider.transform.position.z - 1);
            var spawnedText = LeanPool.Spawn(stackTextVFX, spawnPos,
                Quaternion.identity, stickmanCollider.transform);
            MoveTextY(spawnedText.transform, 3);
        }

        private void MaxCapacityTextShow(bool change) {
            if (change) {
                var spawnPos = new Vector3(stickmanCollider.transform.position.x, stickmanCollider.transform.position.y + 1.5f,
                                stickmanCollider.transform.position.z - 1);
                var spawnedText = LeanPool.Spawn(maxCapacityTextVFX, spawnPos,
                    Quaternion.identity, stickmanCollider.transform);
                MoveTextY(spawnedText.transform, 3.5f);
            }
        }

        private void MoveTextY(Transform trans, float yPos) {
            trans.transform.DOLocalMoveY(yPos, 0.3f).OnComplete(() => { LeanPool.Despawn(trans); });
        }

        private void MakeTrail() {
            var pos = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.499f, transform.localPosition.z);

            points.Add(pos);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, pos);

            if (points.Count > maxPoints) {
                points.RemoveAt(0);
                lineRenderer.positionCount--;
                lineRenderer.SetPositions(points.ToArray());
            }
        }

        public void ResetLineRendererZ() {
            for (int i = 0; i < points.Count; i++) {
                Vector3 newPos = points[i];
                newPos.z -= LevelGenerator.Instance.SpawnOffset;
                points[i] = newPos;
            }
            lineRenderer.SetPositions(points.ToArray());
        }
    }
}